using System;
using System.Linq.Expressions;


namespace Expessions
{

    class Program
    {
        static void Main(string[] args)
        {
            var first = "bc614e";
            var second = "343efcea";
            var result = ExpressionWorkflow()(first, second);
            Console.WriteLine(result);
            Console.ReadLine();
        }


        public static Func<string, string, string> ExpressionWorkflow()
        {
            // expressionVersion
            ConstantExpression map = Expression.Constant(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' }, typeof(char[]));

            var parametr1 = Expression.Parameter(typeof(string), "parametr1");
            var parametr2 = Expression.Parameter(typeof(string), "parametr2");

            var reverseOperation = typeof(Helper).GetMethod(nameof(Helper.ToCharArrayAndReverse), new[] { typeof(string) });
            var reverseOperationHex1Call = Expression.Call(reverseOperation, parametr1);
            var reverseOperationHex2Call = Expression.Call(reverseOperation, parametr2);

            var maxLengthOperation = typeof(Helper).GetMethod(nameof(Helper.MaxLength), new[] { typeof(string), typeof(string) });
            var maxLengthCall = Expression.Call(maxLengthOperation, parametr1, parametr2);
            ConstantExpression wholePart = Expression.Constant(0, typeof(int));

            var newСharOperation = typeof(Helper).GetMethod(nameof(Helper.NewСhar), new[] { typeof(int) });
            var newСharOperationCall = Expression.Call(newСharOperation, maxLengthCall);

            var loopOperation = typeof(Helper).GetMethod(nameof(Helper.LoopResult), new[] { typeof(char[]), typeof(char[]), typeof(char[]), typeof(int), typeof(int), typeof(char[]) });
            var loopOperationCall = Expression.Call(loopOperation, map, reverseOperationHex1Call, reverseOperationHex2Call, maxLengthCall, wholePart, newСharOperationCall);

            var toReverseAndToStringOperation = typeof(Helper).GetMethod(nameof(Helper.ToReverseAndToString), new[] { typeof(char[])});
            var toReverseAndToStringOperationCall = Expression.Call(toReverseAndToStringOperation, loopOperationCall);

            var lambda = Expression.Lambda<Func<string, string, string>>(toReverseAndToStringOperationCall, parametr1, parametr2);
            return lambda.Compile();
        }


        public static string Sum(string hex1, string hex2)
        {
            // baseVersion
            var map = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            var hex1Array = hex1.ToCharArray();
            Array.Reverse(hex1Array);
            var hex2Array = hex2.ToCharArray();
            Array.Reverse(hex2Array);

            var maxLength = Math.Max(hex1.Length, hex2.Length);
            var sum = new char[maxLength + 1];

            var wholePart = 0;

            for (var i = 0; i < maxLength; i++)
            {
                var decimal1 = i > hex1Array.Length - 1 ? 0 : Array.IndexOf(map, hex1Array[i]);
                var decimal2 = i > hex2Array.Length - 1 ? 0 : Array.IndexOf(map, hex2Array[i]);
                var sumPosition = decimal1 + decimal2 + wholePart;
                var remainder = sumPosition % 16;
                wholePart = sumPosition / 16;
                sum[i] = map[remainder];
            }

            sum[maxLength - 1] = wholePart > 0 ? map[wholePart] : '0';

            Array.Reverse(sum);

            return new string(sum);
        }
    }

    internal static class Helper
    {

        public static char[] NewСhar(int maxLength) => new char[maxLength + 1];
        public static int MaxLength(string hex1, string hex2) => Math.Max(hex1.Length, hex2.Length);

        public static char[] ToCharArrayAndReverse(string hex1)
        {
            var hex1Array = hex1.ToCharArray();
            Array.Reverse(hex1Array);
            return hex1Array;
        }

        public static string ToReverseAndToString(char[] arr)
        {
            Array.Reverse(arr);
            return new string(arr);
        }

        public static char[] LoopResult(char[] map, char[] hex1Array, char[] hex2Array, int maxLength, int wholePart, char[] sum)
        {
            for (var i = 0; i < maxLength; i++)
            {
                var decimal1 = i > hex1Array.Length - 1 ? 0 : Array.IndexOf(map, hex1Array[i]);
                var decimal2 = i > hex2Array.Length - 1 ? 0 : Array.IndexOf(map, hex2Array[i]);
                var sumPosition = decimal1 + decimal2 + wholePart;
                var remainder = sumPosition % 16;
                wholePart = sumPosition / 16;
                sum[i] = map[remainder];
            }

            return sum;
        }

    }

}

