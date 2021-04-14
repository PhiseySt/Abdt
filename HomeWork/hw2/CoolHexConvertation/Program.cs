using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HexConvertation
{
    class Program
    {
        static void Main(string[] args)
        {
            var first = "bc614e";
            var second = "343efcea";
            
            var expressionSum = GetSumFunction();

            var result = Sum(first, second);
            var expressionResult = expressionSum(first, second);

            Console.WriteLine($"Simple result: {result}, Expression result: {expressionResult}");
        }
        
        public static string Sum(string hex1, string hex2)
        {
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

            sum[^1] = wholePart > 0 ? map[wholePart] : '0';

            Array.Reverse(sum);

            return new string(sum);
        }

        public static Func<string, string, string> GetSumFunction()
        {
            var hex1 = Expression.Parameter(typeof(string), "hex1");
            var hex2 = Expression.Parameter(typeof(string), "hex2");
            var result = Expression.Parameter(typeof(string), "result");
            
            var map = Expression.Constant(new[]
                {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'});

            var hex1Array = Expression.Variable(typeof(char[]), "hex1Array");
            var hex2Array = Expression.Variable(typeof(char[]), "hex2Array");

            var maxLength = Expression.Variable(typeof(int), "maxLength");
            var sum = Expression.Variable(typeof(char[]), "sum");

            var iterationVariable = Expression.Variable(typeof(int), "iterationVariable");
            var wholePart = Expression.Variable(typeof(int), "wholePart");
            var decimal1 = Expression.Variable(typeof(int), "decimal1");
            var decimal2 = Expression.Variable(typeof(int), "decimal2");
            var sumPosition = Expression.Variable(typeof(int), "sumPosition");
            var remainder = Expression.Variable(typeof(int), "remainder");
            
            var mapReminderArrayAccessExpression = Expression.ArrayAccess(map, remainder);
            var mapWholePartArrayAccessExpression = Expression.ArrayAccess(map, wholePart);
            var hex1ArrayAccessExpression = Expression.ArrayAccess(hex1Array, iterationVariable);
            var hex2ArrayAccessExpression = Expression.ArrayAccess(hex2Array, iterationVariable);
            var sumArrayAccessExpression = Expression.ArrayAccess(sum, iterationVariable);

            var toCharArrayMethod = typeof(string).GetMethod(nameof(string.ToCharArray),
                BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new Type[]{}, null);
            var reverseMethod = typeof(Array).GetMethod(nameof(Array.Reverse), new []{typeof(Array)});
            var indexOfMethod = typeof(Array).GetMethod(nameof(Array.IndexOf), new []{typeof(Array), typeof(Object)});
            var maxMethod = typeof(Math).GetMethod(nameof(Math.Max), new []{typeof(int), typeof(int) });
            var lengthMethod = typeof(string).GetProperty(nameof(string.Length))!.GetGetMethod();
            var lengthArrayMethod = typeof(Array).GetProperty(nameof(Array.Length))!.GetGetMethod();

            var stringConstructor = typeof(string).GetConstructor(new[] {typeof(char[])});

            var writeLineMethod = typeof(Console).GetMethod(nameof(Console.WriteLine), new []{typeof(object)});

            var loopSumLabel = Expression.Label(typeof(char[]));

            var block = Expression.Block(
                new[]
                {
                    hex1Array, hex2Array, iterationVariable, maxLength, sum, wholePart, decimal1, decimal2, sumPosition,
                    remainder, result
                },
                Expression.Assign(hex1Array, Expression.Call(hex1, toCharArrayMethod)),
                Expression.Call(reverseMethod, hex1Array),
                Expression.Assign(hex2Array, Expression.Call(hex2, toCharArrayMethod)),
                Expression.Call(reverseMethod, hex2Array),
                Expression.Assign(maxLength, Expression.Call(maxMethod, Expression.Call(hex1, lengthMethod), Expression.Call(hex2, lengthMethod))),
                Expression.Assign(sum, Expression.NewArrayBounds(typeof(char), Expression.Add(maxLength, Expression.Constant(1)))),
                Expression.Assign(result, Expression.Constant("sum")),
                Expression.Loop(
                    Expression.Block(
                        Expression.IfThenElse(
                                Expression.GreaterThan(iterationVariable,
                                    Expression.Subtract(Expression.Call(hex1Array, lengthArrayMethod),
                                        Expression.Constant(1))),
                                Expression.Assign(decimal1, Expression.Constant(0)),
                                Expression.Assign(decimal1, Expression.Call(indexOfMethod, map, Expression.Convert(hex1ArrayAccessExpression, typeof(object))))),
                        Expression.IfThenElse(
                                Expression.GreaterThan(iterationVariable,
                                    Expression.Subtract(Expression.Call(hex2Array, lengthArrayMethod),
                                        Expression.Constant(1))),
                                Expression.Assign(decimal2, Expression.Constant(0)),
                                Expression.Assign(decimal2, Expression.Call(indexOfMethod, map, Expression.Convert(hex2ArrayAccessExpression, typeof(object))))),
                        Expression.Call(writeLineMethod, Expression.Convert(iterationVariable, typeof(object))),
                        Expression.Assign(sumPosition, Expression.Add(Expression.Add(decimal1, decimal2), wholePart)),
                        Expression.Assign(remainder, Expression.Modulo(sumPosition, Expression.Constant(16))),
                        Expression.Assign(wholePart, Expression.Divide(sumPosition, Expression.Constant(16))),
                        Expression.Assign(sumArrayAccessExpression, mapReminderArrayAccessExpression),
                    Expression.IfThenElse(
                        Expression.LessThan(iterationVariable, maxLength),
                        Expression.PostIncrementAssign(iterationVariable),
                        Expression.Break(loopSumLabel, sum)
                    )),
                    loopSumLabel
                ),
                Expression.IfThenElse(Expression.GreaterThan(wholePart, Expression.Constant(0)),
                    Expression.Assign(sumArrayAccessExpression, mapWholePartArrayAccessExpression),
                    Expression.Assign(sumArrayAccessExpression, Expression.Constant('0'))
                ),
                Expression.Call(reverseMethod, sum),
                Expression.Assign(result, Expression.New(stringConstructor, sum))
            );

            return Expression.Lambda<Func<string, string, string>>(block, hex1, hex2).Compile();
        }
    }
}
