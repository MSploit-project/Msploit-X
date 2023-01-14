using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace Msploit_X.Controls
{
    public partial class HtmlEditor : UserControl
    {
        private readonly TextEditor _textEditor;
        
        public HtmlEditor()
        {
            InitializeComponent();
            _textEditor = this.FindControl<TextEditor>("textCode");
            _textEditor.ShowLineNumbers = true;
            _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
            
            var  _registryOptions = new RegistryOptions(ThemeName.DarkPlus);

            var _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);

            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".html").Id));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}