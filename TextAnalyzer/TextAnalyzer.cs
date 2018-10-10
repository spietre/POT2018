using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{
    class TextAnalyzer
    {
        private bool _isCF;
        private bool _isWF;
        private string _text;
        private int _charCount; //characters count without whitespaces
        private int _charCountWhite; //characters count with whitespaces
        private int _vowelCount; //number of vowels
        private int _consonCount;
        private int _wordCount;
        private int _uniqueWordsCount;
        private int _sentenceCount;

        TextAnalyzer(string[] input)
        {
            _isCF = false;
            _isWF = false;
            _charCount = 0;
            _charCountWhite = 0;
            _vowelCount = 0;
            _consonCount = 0;
            _text = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("-"))
                {
                    switch (input[i].ToLower())
                    {
                        case "-cf":
                            this._isCF = true;
                            break;
                        case "-wf":
                            this._isWF = true;
                            break;
                    }
                }
                else
                {
                    if (_text.Length == 0)
                    {
                        _text = input[i];
                    }
                }
            }
        }

        // writes out status
        public void GetStatus()
        {
            Console.WriteLine("isCF: " + _isCF);
            Console.WriteLine("isWF: " + _isWF);
            Console.WriteLine("Text: " + _text);
        }

        public void analyze()
        {
        }

        /**
         * returns number of characters in text including white-spaces
         */
        public int Length()
        {
            _charCountWhite = _text.Length;

            return _charCountWhite;
        }

        /**
        * returns number of characters in text NOT including white-spaces
        */
        public int LenghtNotWhite()
        {
            _charCount = _text.Replace(" ", "").Length;

            return _charCount;
        }

        // returns number of vowels
        public int VowelCount()
        {
            _vowelCount = 0;
            string lowerText = _text.ToLower();

            for (int i = 0; i < lowerText.Length; i++)
            {
                switch (lowerText[i])
                {
                    case 'a':
                    case 'e':
                    case 'o':
                    case 'u':
                    case 'y':
                    case 'ô':
                        _vowelCount++;
                        break;
                    case 'i':
                        _vowelCount++;
                        switch (lowerText[i + 1])
                        {
                            case 'a':
                            case 'e':
                            case 'u':
                                i++;
                                break;
                        }

                        break;
                    default:
                        continue;
                }
            }

            return _vowelCount;
        }

        // returns number of consonants
        public int ConsonantCount()
        {
            _consonCount = 0;
            string lowerText = _text.ToLower();

            foreach (char c in lowerText)
            {
                if (c < 'a' || c > 'z') continue;

                switch (c)
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                    case 'y':
                    case 'ô':
                        continue;
                    default:
                        _consonCount++;
                        break;
                }
            }

            return _consonCount;
        }

        // returns number of words
        public int WordCount()
        {
            if (_text.Length == 0)
            {
                return _wordCount;
            }

            string[] wordList = _text.ToLower().Split(new [] {' ', ',', '.', '?', '!'}, StringSplitOptions.RemoveEmptyEntries);
            _wordCount = wordList.Length;

            return _wordCount;
        }

        // returns number of unique words
        public int UniqueWordsCount()
        {
            if (_text.Length == 0)
            {
                return _wordCount;
            }

            string[] wordList = _text.ToLower().Split (new[] { ' ', ',', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> uniqueWordSet = new HashSet<string>();

            for (int i = 0; i < wordList.Length; i++)
            {
                uniqueWordSet.Add(wordList[i]);
            }

            _uniqueWordsCount = uniqueWordSet.Count;

            return _uniqueWordsCount;
        }

        // returns number of sentences
        public int SentenceCount()
        {
            if (_text.Length == 0)
            {
                return _wordCount;
            }

            string[] sentenceList = _text.Split(new [] {'.', '!', '?'});

            if (sentenceList[sentenceList.Length - 1].Length == 0)
            {
                _sentenceCount = sentenceList.Length - 1;
            }
            else
            {
                _sentenceCount = sentenceList.Length;
            }

            return _sentenceCount;
        }

        // returns average number of words in one sentence
        public double AvgWordCount()
        {
            if (_sentenceCount == 0)
            {
                return 0;
            }

            double avgWrdCnt = (double) _wordCount / (double) _sentenceCount;

            return Math.Round(avgWrdCnt, 2);
        }

        // Calculates frequency of characters in the given text string
        public void CharFreq()
        {
            if (!_isCF || _text.Length == 0)
            {
                return;
            }

            Dictionary<char, int> charFreqDictionary = new Dictionary<char, int>();

            foreach (char c in _text.ToLower())
            {
                if (c > 'z' || c < 'a')
                {
                    // character has to be a letter
                    continue;
                }

                charFreqDictionary.TryGetValue(c, out int frequency);
                charFreqDictionary[c] = ++frequency;
            }

            List<KeyValuePair<char, int>> orderedList = charFreqDictionary.ToList();
            orderedList.Sort(CharFreqComparer);

            // writing to stdout
            Console.WriteLine("\nCharacter frequencies: ");
            foreach (KeyValuePair<char, int> pair in orderedList)
            {
                Console.WriteLine(pair.Key + ": " + pair.Value + "x");
            }
        }

        // Help function
        private static int CharFreqComparer(KeyValuePair<char, int> one, KeyValuePair<char, int> two)
        {
            // at first order by values
            if (one.Value > two.Value)
            {
                return -1;
            }

            if (one.Value < two.Value)
            {
                return 1;
            }

            // if values are equal then order by keys
            if (one.Key < two.Key)
            {
                return -1;
            }

            if (one.Key > two.Key)
            {
                return 1;
            }

            return 0;
        }

        // Help function
        private static int WordFreqComparer (KeyValuePair<string, int> one, KeyValuePair<string, int> two)
        {
            // at first order by values
            if (one.Value > two.Value)
            {
                return -1;
            }

            if (one.Value < two.Value)
            {
                return 1;
            }

            return 0;
        }

        public void WordFreq ()
        {
            if (!_isWF || _text.Length == 0) 
            {
                return;
            }

            string[] wordList = _text.ToLower().Split(new[] {' ', '!', '?', '.', ','}, StringSplitOptions.RemoveEmptyEntries);
            //Replace(",", "").Replace(".", "").Replace("!", "").Replace("?", "")
            //.Split (' ', '!', '?');

            Dictionary<string, int> worDictionary = new Dictionary<string, int>();

            for (int i = 0; i < wordList.Length; i++)
            {
                    worDictionary.TryGetValue(wordList[i], out int frequency);
                    worDictionary[wordList[i]] = ++frequency;
            }

            List<KeyValuePair<string, int>> sortedList = worDictionary.ToList();
            sortedList.Sort (WordFreqComparer);

            // writing to stdout
            Console.WriteLine ("\nWord frequencies: ");
            foreach (KeyValuePair<string, int> pair in sortedList)
            {
                Console.WriteLine (pair.Key + ": " + pair.Value + "x");
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments entered");
                Console.WriteLine("Usage: TextAnalyzer.exe <switch> <args>\n");
                Console.WriteLine("\tswitches:\n\t-wf\n\t-cf\n");
                Console.WriteLine("\targs: string of characters");
                Console.ReadLine();
                return;
            }


            TextAnalyzer analyzer = new TextAnalyzer(args);

#if DEBUG
            analyzer.GetStatus();
#endif
            Console.WriteLine("Number of characters (with spaces):\t" + analyzer.Length());
            Console.WriteLine("Number of characters (no spaces):\t" + analyzer.LenghtNotWhite());
            Console.WriteLine("Number of vowels:\t\t\t" + analyzer.VowelCount());
            Console.WriteLine("Number of consonants:\t\t\t" + analyzer.ConsonantCount());
            Console.WriteLine("Number of words:\t\t\t" + analyzer.WordCount());
            Console.WriteLine("Number of unique words:\t\t\t" + analyzer.UniqueWordsCount());
            Console.WriteLine("Number of sentences:\t\t\t" + analyzer.SentenceCount());
            Console.WriteLine("Average sentence length (words):\t" + analyzer.AvgWordCount());

            analyzer.CharFreq();
            analyzer.WordFreq ();


            Console.WriteLine("Press Enter To Exit...");
            Console.ReadLine();
        }
    }


}
