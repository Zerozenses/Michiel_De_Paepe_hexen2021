using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HEX.GameSystem
{
    public class GameLoop : MonoBehaviour
    {
       #region fields
        //essentials
        public event EventHandler Initialized;
        [SerializeField]
        public PositionHelper _positionHelper = null;

        //game objects
        [SerializeField]
        public GameObject _hex;
        [SerializeField]
        public GameObject _carddeck;
        [SerializeField]
        public GameObject _playerModel;
        [SerializeField]
        public GameObject _enemyModel;

        //amounts
        [SerializeField]
        public int _radius = 3;
        [SerializeField]
        public int _amountEnemies =0;
        [SerializeField]
        public int _sizeDeck=13;

        //images typecards
        [SerializeField]
        public GameObject SwipeCard;
        [SerializeField]
        public GameObject KnockbackCard;
        [SerializeField]
        public GameObject SwingCard;
        [SerializeField]
        public GameObject TeleportCard;
        [SerializeField]
        public GameObject BombCard;

        //others
        private Grid<Position> _grid;
        private Board<Position, ICharacter> _board;
        private Deck<Card> _deck;
        private Position _currentPos;
        private MoveManager _movementManager;
        private Player _player;
        private Card _currentCard;
        #endregion

        public void Start()
        {
            //initialisations
            _grid = new Grid<Position>(_radius*2+2, _radius*2+2);
            _board = new Board<Position, ICharacter>();
            _deck = new Deck<Card>();

            //Set up hexes
            CreateBoard();

            ConnectPlayer();
            _board.Player = _player;

            _movementManager = new MoveManager(_board, _grid);

            //Set amount of enemies on random spots
            GenerateEnemies();

            //Deck
            //1.Initialize card deck
            GenerateDeck();

            //2.Draw cards (draw five at first)
            #region draw cards originally
            //originally the idea was doing the drawing of them one by one but it's better to do it all at once

            /*DrawCard();
            DrawCard();
            DrawCard();
            DrawCard();
            DrawCard();*/
            #endregion
            DrawCards();

            //3.Action manager?
            BoardListeners();
            GridLogic();
            CardLogic(); //Actions of the cards
                         //Action manager for enemies and card effects

            //Replaymanager?

            OnInitialized(EventArgs.Empty);
        }

        //On start game
        protected virtual void OnInitialized(EventArgs arg)
        {
            EventHandler handler = Initialized;
            handler?.Invoke(this, arg);
        }

        #region board
        //create board
        private void CreateBoard()
        {
            //logic behind the creation of the board, from website
            #region logic 
            //to get the figure from website
            /*for (int q = -map.radius; q <= map_radius; q++)
            {
                int r1 = max(-map_radius, -q - map_radius);
                int r2 = min(map_radius, -q + map_radius);
                for (int r = r1; r <= r2; r++)
                {
                    grid.insert(Hex(q, r, -q - r));
                }
            }*/
            #endregion 

            for (int q = -_radius; q <= _radius; q++)
            {
                int rMax = Mathf.Max(-_radius, -q - _radius);
                int rMin = Mathf.Min(_radius, -q + _radius);
                for (int r = rMax; r <= rMin; r++)
                {
                    //determine hex location through calculations
                    //Vector3 hexPosition = CreateHex(q, r, 0);

                    //determine position

                    //float s = -q * 1.8f - r * 1.8f;
                    //Vector3 position = new Vector3(hexPosition.x, 0, hexPosition.y);
                    //Vector3 position = new Vector3(hexPosition.x, 0, hexPosition.y);
                    Vector3 position = _positionHelper.ToWorldPosition(q, r);
                    //var Position = new Position(q, r, s);
                    float s = -q  - r;
                    var Position = new Position(q, r, s);
                    //var Position = new Position((int)hexPosition.x, 0, (int)hexPosition.y);

                    //instantiate hex
                    GameObject hex = Instantiate(_hex, position, Quaternion.identity);
                    hex.name = $"Hex {q}, {r}, {s}";
                    Hex tile = hex.GetComponent<Hex>();
                    tile.Position = Position;

                    //give deck currentcard when played
                    tile.CardHexEntered += (s, e) =>
                    {
                        _currentCard = e.CurrentCard;
                    };

                    //register in grid
                    _grid.Register(q, r, Position);
                }
            }
        }

        //create hex
        private Vector3 CreateHex(float q, float r, float s)
        {
            var x = (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = (3f / 2f * r);
            var z = s;

            return new Vector3(x, y, z);
        }

        private void BoardListeners()
        {
            
            _board.Placed += (s, e) =>
            {
                if (_grid.TryGetCoordinateAt(e.ToPosition, out var toCoordinate))
                {
                    Vector3 worldPos = _positionHelper.ToWorldPosition(toCoordinate.x, toCoordinate.y);
                    Debug.Log(e.Piece.CharacterType + " placed on " + worldPos);
                    //_board.Place(e.Piece, new Position((int)(worldPos.x), (int)worldPos.y, (int)worldPos.z));
                    (e.Piece as Character).gameObject.transform.position = worldPos;

                    //e.ToPosition
                }
            };

            _board.Pushed += (s, e) =>
            {
                if (_grid.TryGetCoordinateAt(e.ToPosition, out var toCoordinate))
                {
                    Vector3 worldPos = _positionHelper.ToWorldPosition(toCoordinate.x, toCoordinate.y);
                    Debug.Log(e.Piece.CharacterType + " pushed  to " + worldPos);
                    (e.Piece as Character).gameObject.transform.position = worldPos;
                    //e.Piece.Position = new Position(worldPos.x, worldPos.y, worldPos.z);
                }
            };

            _board.Taken += (s, e) =>
            {
                Debug.Log(e.Piece.CharacterType + " was found");
                e.Piece.Hitting();
            };

        }

        private void GridLogic()
        {
            Hex[] hexes = FindObjectsOfType<Hex>();
            foreach (var tile in hexes)
            {
                tile.CardHexEntered += (s, e) =>
                {
                    _currentCard = e.CurrentCard;
                    List<Position> validPositions = _movementManager.ValidPositions(_currentCard, _board.Player, tile.Position);
                    List<Position> isolatedPositions = _movementManager.IsolatedPositions(_currentCard, _board.Player, tile.Position);

                    if (isolatedPositions.Contains(tile.Position))
                    {
                        tile.IsHighlighted = true;
                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {
                            foreach (var position in isolatedPositions)
                            {
                                if (position.Equals(hex.Position))
                                {
                                    hex.IsHighlighted = true;
                                }
                            }
                        }
                    }
                    else 
                    {
                        //tile.IsHighlighted = true;
                        //Debug.Log(_player.Position.X + "+" + _player.Position.Y + "+" + _player.Position.Z);

                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {
                            foreach (var position in validPositions)
                            {
                                if (position.Equals(hex.Position))
                                {
                                    hex.IsHighlighted = true;
                                }
                            }
                        }
                    }

                    _currentCard.Selected += (s, e) =>
                    {

                    };

                };

                tile.CardHexDropped += (s, e) =>
                {
                    List<Position> validPositions = _movementManager.ValidPositions(_currentCard, _board.Player, tile.Position);
                    List<Position> isolatedPositions = _movementManager.IsolatedPositions(_currentCard, _board.Player, tile.Position);

                    if (isolatedPositions.Contains(tile.Position))
                    {
                        tile.IsHighlighted = false;
                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {
                            foreach (var position in isolatedPositions)
                            {
                                if (position.Equals(hex.Position))
                                {
                                    hex.IsHighlighted = false;
                                    _movementManager.Move(_currentCard, _board.Player, hex.Position);
                                    if(_currentCard.Type == CardType.Bomb)
                                    {
                                        //temporary to check if working
                                        hex.Destruction();
                                    }
                                    _deck.PlayCard(_currentCard, hex.Position);
                                    _currentCard.Used();
                                }
                            }
                        }
                    }
                    else 
                    {
                        //tile.IsHighlighted = false;
                        //Debug.Log(_player.Position.X + "+" + _player.Position.Y + "+" + _player.Position.Z);

                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {

                             hex.IsHighlighted = false;

                        }
                    }

                };

                tile.CardHexExited += (s, e) =>
                {
                    var validPositions = _movementManager.ValidPositions(_currentCard, _player, tile.Position);
                    var isolatedPositions = _movementManager.IsolatedPositions(_currentCard, _player, tile.Position);

                    if (isolatedPositions.Contains(tile.Position))
                    {
                        //tile.IsHighlighted = false;
                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {
                            hex.IsHighlighted = false;
                        }
                    }
                    else
                    {
                        //tile.IsHighlighted = false;
                        //Debug.Log(_player.Position.X + "+" + _player.Position.Y + "+" + _player.Position.Z);

                        //dehighlights other hexes
                        Hex[] _hexes = FindObjectsOfType<Hex>();
                        foreach (var hex in _hexes)
                        {
                             hex.IsHighlighted = false;
                        }
                    }

                    _currentCard.Deselected += (s, e) =>
                    {
                        _currentCard = null;
                    };
                };
            }
        }
        #endregion

        #region deck
        //generate random deck
        private void GenerateDeck()
        {
            for (int i = 0; i < _sizeDeck; i++)
            {
                GenerateCard();
            }
        }

        //generate cards
        private void GenerateCard()
        {
            switch (Random.Range(1, 6))
            {
                case 1:
                    var knockbackCard = Instantiate(KnockbackCard, _carddeck.transform);
                    Card cardKnockback = knockbackCard.GetComponent<Card>();
                    cardKnockback.Type = CardType.Knockback;
                    //cardKnockback.Board = _board;
                    //cardKnockback.Grid = _grid;
                    _deck.RegisterCard(cardKnockback);
                    break;
                case 2:
                    var swingbackCard = Instantiate(SwingCard, _carddeck.transform);
                    Card cardSwing = swingbackCard.GetComponent<Card>();
                    cardSwing.Type = CardType.Swing;
                    //cardSwing.Board = _board;
                    //cardSwing.Grid = _grid;
                    _deck.RegisterCard(cardSwing);
                    break;
                case 3:
                    var swipeCard = Instantiate(SwipeCard, _carddeck.transform);
                    Card cardSwipe = swipeCard.GetComponent<Card>();
                    cardSwipe.Type = CardType.Swipe;
                    //cardSwipe.Board = _board;
                    //cardSwipe.Grid = _grid;
                    _deck.RegisterCard(cardSwipe);
                    break;
                case 4:
                    var teleportCard = Instantiate(TeleportCard, _carddeck.transform);
                    Card cardTeleport = teleportCard.GetComponent<Card>();
                    cardTeleport.Type = CardType.Teleport;
                    //cardTeleport.Board = _board;
                    //cardTeleport.Grid = _grid;
                    _deck.RegisterCard(cardTeleport);
                    break;
                case 5:
                    var bombCard = Instantiate(BombCard, _carddeck.transform);
                    Card cardBomb = bombCard.GetComponent<Card>();
                    cardBomb.Type = CardType.Bomb;
                    _deck.RegisterCard(cardBomb);
                    break;
            }
        }

        //draw card
        private void DrawCards()
        {
            _deck.FillDeck();
        }

        //card actions
        private void CardLogic()
        {


        }
        #endregion

        #region characters
        private void ConnectPlayer()
        {
            Vector3 position = new Vector3(0, 0, 0);
            GameObject character = Instantiate(_playerModel, position, Quaternion.identity, transform);
            _player = character.GetComponent<Player>();
            _player.Position = new Position(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z);
            _board.Place(_player, _player.Position);
            
        }

        //generate enemies on random spots
        private void GenerateEnemies()
        {
            //amount of enemies given to add on the board
            //can't be on player position or same one
            for (int i = 0; i < _amountEnemies; i++)
            {
                GenerateEnemy();
            }
        }

        //generate enemy
        private void GenerateEnemy()
        {
            Hex[] tiles = FindObjectsOfType<Hex>();
            int index = Random.Range(0, tiles.Length);
            Hex selectedTile = tiles[index];

            //prevent from spawning in player position
            if (selectedTile.name == "Hex 0, 0, 0"){
                index = Random.Range(0, tiles.Length);
                selectedTile = tiles[index];
            }

            Vector3 worldPos = _positionHelper.ToWorldPosition((int)selectedTile.Position.X, (int)selectedTile.Position.Y);
            var enemy = Instantiate(_enemyModel, worldPos, Quaternion.identity, transform);
            Enemy enemyCharacter = enemy.GetComponent<Enemy>();

            enemyCharacter.CharacterType = CharacterType.Enemy;
            enemyCharacter.transform.position = selectedTile.transform.position;

            _board.Place(enemyCharacter, selectedTile.Position);

        }
        #endregion
    }
}
