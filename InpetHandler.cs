using System.Collections.Generic;
using System.Windows.Input;

namespace Sluggity {
    internal class InpetHandler {

        private readonly MainWindow _mainWindow;
        private static HashSet<Key> _keysPressed = new HashSet<Key>();

        public InpetHandler(MainWindow mainWindow) {
            _mainWindow = mainWindow;
            _mainWindow.KeyDown += HandleKeyDown;
            _mainWindow.KeyUp += HandleKeyUp;
        }

        private void HandleKeyDown(object sender, KeyEventArgs e) {
            _keysPressed.Add(e.Key);
        }

        private void HandleKeyUp(object sender, KeyEventArgs e) {
            _keysPressed.Remove(e.Key);
        }

        public static bool GetKeyDown(Key key) {
            return _keysPressed.Contains(key);
        }

        public static bool GetKeyUp(Key key) {
            return !_keysPressed.Contains(key);
        }

        public static bool GetKey(Key key) {
            return _keysPressed.Contains(key);
        }
    }
}
