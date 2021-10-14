using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BETSI
{
    class Calculator
    {
        public static string Calc(string enter)
        {
            ///////EXEMPLO///////
            // How much is five "cinco" plus five "cinco"
            // Break spacevoids
            /*
             * 0 - How
             * 1 - much is
             * 2 - five "cinco"
             * 3 - divido
             * 4 - por
             * 5 - five "cinco"
             */

            string[] parts = enter.Split(' ');
            double n1 = Grammars.DicNumbers[parts[2]];
            double n2;
            double result = 0;

            try
            {
                n2 = Grammars.DicNumbers[parts[4]];
            }
            catch (KeyNotFoundException)
            {
                n2 = Grammars.DicNumbers[parts[5]]; //If you don't find the key in the dictionary, the index becomes 5
            }

            try
            {
                switch (parts[3])
                {
                    case "mais":
                        result = n1 + n2;
                        break;

                    case "menos":
                        result = n1 - n2;
                        break;

                    case "vezes":
                        result = n1 * n2;
                        break;

                    case "divido":
                        result = n1 / n2;
                        break;

                    case "elevado":
                        result = Math.Pow(n1, n2);
                        break;

                    case "raiz":
                        result = Math.Pow(n2, 1 / n1);
                        break;
                }
            }
            catch (Exception)
            {
                
              /*  if( speak.Any(f => f == "Quanto é 0 dividido por 0"))
                {
                    return "Zero dividido por zero tende a infinito!!!";
                }
                else */
                    return "Cálculo inválido!";
            }

            return Math.Round(result, 2).ToString(); // Case dizim periodic
        }
    }
}
