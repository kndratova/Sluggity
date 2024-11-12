using Sluggity.Core;
using System.Windows.Controls;

namespace Sluggity.Pages
{
    public partial class Level1 : Page
    {
        public Level1()
        {
            InitializeComponent();
            GameCore.Instance.Construct(GameCanvas);
            SceneManager.ChangeScene("Scene_0");
        }
    }
}
