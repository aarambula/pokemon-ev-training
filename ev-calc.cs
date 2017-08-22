using System;
using System.Collections.Generic;

namespace ConsoleApplication2
{
    class Program
    {
        static List<EVElementsTable> lEVElements;

        static void Main(string[] args)
        {
            InitEVElements();

            foreach (KeyValuePair<string, int> dPokemonToDefeat in CalculatePokemonToDefeat(98, 1, 0, true, true, true, true))
            {
                Console.WriteLine("Conditions = {0}, Pokemon = {1}", dPokemonToDefeat.Key, dPokemonToDefeat.Value);
            }
        }

        static void InitEVElements()
        {
            lEVElements = new List<EVElementsTable>();
            lEVElements.Add(new EVElementsTable(true, true, true));
            lEVElements.Add(new EVElementsTable(true, true, false));
            lEVElements.Add(new EVElementsTable(true, false, true));
            lEVElements.Add(new EVElementsTable(true, false, false));
            lEVElements.Add(new EVElementsTable(false, true, true));
            lEVElements.Add(new EVElementsTable(false, true, false));
            lEVElements.Add(new EVElementsTable(false, false, true));
            lEVElements.Add(new EVElementsTable(false, false, false));
        }

        static Dictionary<string, int> CalculatePokemonToDefeat(int nEVTarget, int nBaseEV = 1, int nVitamins = 0, bool bPokerus = false, bool bSOS = false, bool bPowerItem = false, bool bCalculate = true)
        {
            Dictionary<string, int> dPokemonToDefeat = new Dictionary<string, int>();
            int nEVReminder = nEVTarget - (nVitamins >= 10 ? 100 : nVitamins * 10);
            int nCalcBaseEV = 0;

            foreach (EVElementsTable oEVElement in lEVElements)
            {
                //Calculate with the available elements
                if (bCalculate && ((bPokerus == false && oEVElement.bPokerus)
                    || (bSOS == false && oEVElement.bSOS)
                    || (bPowerItem == false && oEVElement.bPowerItem)))
                {
                    continue;
                }

                nCalcBaseEV = GetBaseEV(nBaseEV, oEVElement.bPokerus, oEVElement.bSOS, oEVElement.bPowerItem);
                int nPokemon = nEVReminder / nCalcBaseEV;
                nEVReminder = nEVReminder % nCalcBaseEV;

                if (nPokemon > 0)
                {
                    dPokemonToDefeat.Add(oEVElement.GetElementLabel(), nPokemon);
                }
            }

            return dPokemonToDefeat;
        }

        static int GetBaseEV(int nEVSpread = 1, bool bPokerus = false, bool bSOS = false, bool bPowerItem = false)
        {
            int nPokerus = bPokerus ? 2 : 1;
            int nPowerItem = bPowerItem ? 8 : 0;
            int nSOS = bSOS ? 2 : 1;
            return (nEVSpread + nPowerItem) * nPokerus * nSOS;
        }
    }

    class EVElementsTable
    {
        public EVElementsTable(bool bPokerus = false, bool bSOS = false, bool bPowerItem = false)
        {
            this.bPokerus = bPokerus; this.bSOS = bSOS; this.bPowerItem = bPowerItem;
        }
        public bool bPokerus { get; set; }
        public bool bSOS { get; set; }
        public bool bPowerItem { get; set; }
        public string GetElementLabel()
        {
            return Convert.ToString("Pokerus:" + bPokerus.ToString() + " SOS:" + bSOS.ToString() + " Power Item:" + bPowerItem.ToString());
        }
    }
}
