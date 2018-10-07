using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{
    class TextAnalyzer
    {
        private bool isCF;
        private bool isWF;
        private string text;
        private int aCharCount; //characters count without whitespaces
        private int aCharCountWhite; //characters count with whitespaces
        private int aVowelCount; //number of vowels
        private int aConsonCount;
        private int aWordCount;
        private int aUniqueWordsCount;
        private int aSentenceCount;

        TextAnalyzer(string[] input)
        {
            isCF = false;
            isWF = false;
            aCharCount = 0;
            aCharCountWhite = 0;
            aVowelCount = 0;
            aConsonCount = 0;
            text = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("-"))
                {
                    switch (input[i].ToLower())
                    {
                        case "-cf":
                            this.isCF = true;
                            break;
                        case "-wf":
                            this.isWF = true;
                            break;
                    }
                }
                else
                {
                    if (text.Length == 0)
                    {
                        text = input[i];
                    }
                }
            }
        }

        public void GetStatus()
        {
            Console.WriteLine("isCF: " + isCF);
            Console.WriteLine("isWF: " + isWF);
            Console.WriteLine("Text: " + text);
        }

        public void analyze()
        {
        }

        /**
         * returns number of characters in text including white-spaces
         */
        public int Length()
        {
            this.aCharCountWhite = text.Length;

            return aCharCountWhite;
        }

        /**
        * returns number of characters in text NOT including white-spaces
        */
        public int LenghtNotWhite()
        {
            this.aCharCount = text.Replace(" ", "").Length;

            return aCharCount;
        }

        public int VowelCount()
        {
            aVowelCount = 0;
            string lowerText = text.ToLower();

            foreach (char c in lowerText)
            {
                switch (c)
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                    case 'y':
                        aVowelCount++;
                        break;
                    default:
                        continue;
                }
            }

            return aVowelCount;
        }

        public int ConsonantCount()
        {
            aConsonCount = 0;
            string lowerText = text.ToLower();

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
                        continue;
                    default:
                        aConsonCount++;
                        break;
                }
            }

            return aConsonCount;
        }

        public int WordCount()
        {
            if (text.Length == 0)
            {
                return aWordCount;
            }

            string[] wordList = text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("?", "")
                .Split(' ');
            aWordCount = wordList.Length;

            return aWordCount;
        }

        public int UniqueWordsCount()
        {
            if (text.Length == 0)
            {
                return aWordCount;
            }

            string[] wordList = text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("?", "")
                .Split(' ');
            HashSet<string> uniqueWordSet = new HashSet<string>();

            for (int i = 0; i < wordList.Length; i++)
            {
                uniqueWordSet.Add(wordList[i]);
            }

            aUniqueWordsCount = uniqueWordSet.Count;

            return aUniqueWordsCount;
        }

        public int SentenceCount()
        {
            if (text.Length == 0)
            {
                return aWordCount;
            }

            string[] sentenceList = text.Replace("?", ".").Replace("!", ".").Split('.');

            if (sentenceList[sentenceList.Length - 1].Length == 0)
            {
                aSentenceCount = sentenceList.Length - 1;
            }
            else
            {
                aSentenceCount = sentenceList.Length;
            }

            return aSentenceCount;
        }

        public double AvgWordCount()
        {
            if (aSentenceCount == 0)
            {
                return 0;
            }

            double avgWrdCnt = (double) aWordCount / (double) aSentenceCount;

            return Math.Round(avgWrdCnt, 2);
        }

        // Calculates frequency of characters in given text string
        public void CharFreq()
        {
            if (!isCF || text.Length == 0)
            {
                return;
            }

            Dictionary<char, int> charFreqDictionary = new Dictionary<char, int>();

            foreach (char c in text.ToLower())
            {
                if (c > 'z' || c < 'a')
                {
                    // character has to be a letter
                    continue;
                }

                try
                {
                    charFreqDictionary.Add(c, 1);
                }
                catch (ArgumentException e)
                {
                    charFreqDictionary[c] += 1;
                }
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
            if (!isWF || text.Length == 0) 
            {
                return;
            }

            string[] wordList = text.ToLower ().Replace (",", "").Replace (".", "").Replace ("!", "").Replace ("?", "")
                .Split (' ');
            
            Dictionary<string, int> worDictionary = new Dictionary<string, int>();

            for (int i = 0; i < wordList.Length; i++)
            {
                try
                {
                    worDictionary.Add(wordList[i], 1);
                }
                catch (ArgumentException e)
                {
                    worDictionary[wordList[i]] += 1;
                }
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
            analyzer.GetStatus();

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
