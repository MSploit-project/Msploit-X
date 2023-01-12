using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using Msploit_X.Behaviors;

namespace Msploit_X.Controls
{
    public partial class CodeEditor : UserControl
    {
        private readonly TextEditor _textEditor;
        
        public CodeEditor()
        {
            InitializeComponent();
            _textEditor = this.FindControl<TextEditor>("TextCode");
            _textEditor.ShowLineNumbers = true;
            _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}