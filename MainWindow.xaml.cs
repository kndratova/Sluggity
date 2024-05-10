using System;
using System.Windows;
using System.Windows.Controls;
using Sluggity.Core;

namespace Sluggity
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.SetOut(new ConsoleOutputWriter(ConsoleOutput));
            GameCore.Construct(GameCanvas);
        }

        private class ConsoleOutputWriter(TextBox output) : System.IO.TextWriter
        {
            public override void Write(char value) => output.Dispatcher.Invoke(() => output.Text = TimerManager.GetElapsedTime() + value + '\n' + output.Text);
            public override void Write(string value) => output.Dispatcher.Invoke(() => output.Text = TimerManager.GetElapsedTime() + value + '\n' + output.Text);
            public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
        }

        private void ConsoleButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConsoleOutput.Visibility = ConsoleOutput.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
