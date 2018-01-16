using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParserGui.Annotations;
using ParserLibrary;
using ParserLibrary.Parsers.Lr;

namespace ParserGui
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompileData _compileData;
        private LrParser _parser;
        public MainWindow()
        {
            _compileData = new CompileData();
            
            DataContext = _compileData;

            InitializeComponent();

            _compileData.CodeToCompile = "= m 45 if := num 10 = 3 3 if := k 6";

            ParserLoader loader = new ParserLoader("words.txt", "rules.txt", "table.txt");
            _parser = new LrParser(loader);
            _parser.OnCompileError += ParserOnOnCompileError;
            _parser.OnCompileDone += ParserOnOnCompileDone;
            _parser.OnCompileNextStep += ParserOnOnCompileNextStep;
            
        }

        private void ParserOnOnCompileNextStep(object sendler, CompileInfoEventArgs e)
        {
            _compileData.CompileInfo += e + Environment.NewLine;
            
        }

        private void ParserOnOnCompileDone(object sendler, CompileInfoEventArgs e)
        {
            _compileData.CompiledCode = _parser.CompiledText;
        }

        private void ParserOnOnCompileError(object sendler, CompileErrorEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _compileData.CompileInfo = String.Empty;
            Thread thr = new Thread(_parser.Run);
            thr.Start(_compileData.CodeToCompile);
        }

        private void CompileDataTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CompileDataTextBox.ScrollToEnd();
        }
    }
}
