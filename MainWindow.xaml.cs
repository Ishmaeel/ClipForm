using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using WK.Libraries.SharpClipboardNS;

namespace ClipForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SharpClipboard _Clipboard = new SharpClipboard();

        public MainWindow()
        {
            InitializeComponent();

            InitializeMonitor();
        }

        private void InitializeMonitor()
        {
            _Clipboard.ClipboardChanged += Clipboard_ClipboardChanged;
        }

        private void Clipboard_ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            if (e.ContentType == SharpClipboard.ContentTypes.Text)
                Process($"{e.Content}");
        }

        private void Process(string content)
        {
            const string pattern = " kartınız ile (.*) firmasından .* saatinde (.*) TL tutarında ";

            var match = Regex.Match(content, pattern);

            if (match.Success && match.Groups.Count == 3)
            {
                var seller = match.Groups[1].Value;
                var amountStr = match.Groups[2].Value;

                if (double.TryParse(amountStr, out var amount))
                {
                    string finalOutput = $"{-amount}\t{seller}";

                    try
                    {
                        Clipboard.SetDataObject(finalOutput);

                        Print(finalOutput);
                    }
                    catch (Exception ex)
                    {
                        Print(ex.Message);
                    }
                }
            }
        }

        private void Print(string message)
        {
            TextOut.Text = message;
            Debug.Print($"[ClipForm] {message}".Replace("\r", "").Replace("\n", ""));
        }
    }
}