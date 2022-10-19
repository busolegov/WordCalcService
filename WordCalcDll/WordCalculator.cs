using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCalcDll
{
    public class WordCalculator
    {
        private readonly int _taskCount = Environment.ProcessorCount;
        private readonly string _text;
        private readonly ConcurrentDictionary<string, int> _map = new ConcurrentDictionary<string, int>();
        public WordCalculator(string text)
        {
            _text = text;
        }

        private int[] GetTextPointers()
        {
            int pointerCount = _taskCount + 1;
            int[] textPointers = new int[pointerCount];
            textPointers[0] = 0;
            textPointers[pointerCount - 1] = _text.Length;

            for (int i = 1; i < textPointers.Length - 1; i++)
            {
                textPointers[i] = (_text.Length / _taskCount) * i;

                while (textPointers[i] < _text.Length && !Char.IsWhiteSpace(_text[textPointers[i]]))
                {
                    ++textPointers[i];
                }
                if (textPointers[i] >= _text.Length)
                {
                    textPointers[i] = _text.Length;
                    continue;
                }
            }
            return textPointers;
        }

        private void CalculateWordsTextPart(int start, int end)
        {
            StringBuilder Word = new StringBuilder();

            for (int i = start; i < end; i++)
            {
                char symbol = _text[i];

                if (Char.IsLetter(symbol) || Char.IsNumber(symbol))
                {
                    Word.Append(char.ToLower(symbol));
                }
                else if ((symbol =='\'' || symbol == '-' || Char.IsNumber(symbol)) && Word.Length > 0)
                {
                    Word.Append(symbol);
                }
                else if (Word.Length > 0)
                {
                    _map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                    Word.Clear();
                }
            }
            if (Word.Length > 0)
            {
                _map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                Word.Clear();
            }
        }

        public Dictionary<string, int> CalculateWordsFullText()
        {
            int[] textPointers = GetTextPointers();

            int start = 0;
            int end = _taskCount-1;

            //Конструкция Parallel.For
            Parallel.For(start, end, i => CalculateWordsTextPart(textPointers[i], textPointers[i+1]));

            return _map.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); ;
        }
    }
}
