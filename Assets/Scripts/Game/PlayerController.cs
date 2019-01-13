using AgistForms.Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private KeyCode _upKey;

        [SerializeField]
        private KeyCode _downKey;

        [SerializeField]
        private KeyCode _leftKey;

        [SerializeField]
        private KeyCode _rightKey;        

        private Dictionary<KeyCode, Commands> _keyCodesToCommands;
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
            _keyCodesToCommands = new Dictionary<KeyCode, Commands>
            {
                { _upKey, Commands.Up },
                { _downKey, Commands.Down },
                { _leftKey, Commands.Left },
                { _rightKey, Commands.Right }
            };
        }

        private void FixedUpdate()
        {
            var pressedCommand = _keyCodesToCommands.Keys.FirstOrDefault(x => Input.GetKey(x));

            if (pressedCommand != KeyCode.None)
            {
                _player.GetCommand(_keyCodesToCommands[pressedCommand]);
            }
        }

    }
}
