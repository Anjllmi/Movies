using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Utilities
{
    public static class OperationExtensions
    {
        ////Función para sumar al estilo LAMBDA 
        //public static Func<int, int, int> CustomSum = (a, b) => a + b;

        //Método que extiende el primer entero.
        public static int CustomSum(this int a, int b)
        {
            return a + b;
        }
    }
}
