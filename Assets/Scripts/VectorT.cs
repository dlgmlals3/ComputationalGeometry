using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;

namespace Computation.geograpy
{
    /// <summary>
    /// Generic class for arithmetic operator overloading demonstration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Vector<T> : IEnumerable<T>
    {
        //static operator delegates
        private static Func<T, T, T> Add = FuncGenerator<T>.CreateOperatorFuncAdd();
        private static Func<T[], T[], T[]> AddT = FuncGenerator<T>.CreateArrayOperatorFuncAdd();

        //http://stackoverflow.com/questions/1717444/combining-two-lamba-expressions-in-c-sharp

        //the ideas for implementation was based on article
        //http://blogs.msdn.com/b/csharpfaq/archive/2009/11/19/debugging-expression-trees-in-visual-studio-2010.aspx
        // 

        private T[] values;
        public Vector(int dim)
        {
            this.values = new T[dim];
        }

        public Vector(IEnumerable<T> initialValues)
        {
            values = initialValues.ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            return AddT(a.values, b.values);
        }

        /// <summary>
        /// Allows easily convert an array to vector of same item type. Can be used with operator overloading.
        /// </summary>
        /// <param name="a">array to be converted</param>
        /// <returns></returns>
        public static implicit operator Vector<T>(T[] a)
        {
            return new Vector<T>(a);
        }

        /// <summary>
        /// IEnumerable&LT;T&GT; interface implementation.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (values as IEnumerable<T>).GetEnumerator();
        }

        /// <summary>
        /// IEnumerable interface implementation.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        /// <summary>
        /// Some kind of conversion of Vector into string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";

            result = this.Select(t => t.ToString()).Aggregate((a, b) => a + ";" + b);
            return "[" + result + "]";

        }

        /// <summary>
        /// Some kind of parsing string into Vector. Items must be delimited by ';' char. On left side '[' char is expected, on right side ']' char is expected.
        /// This is just demonstration class so I am not proud of this implementation as some strange string forms will be successfuly parsed into T[]. 
        /// </summary>
        /// <param name="value">String value which will be parsed.</param>
        /// <param name="parse">Parse delegate for conversion from string into T value.</param>
        /// <returns></returns>
        public static T[] Parse(string value, Func<string, T> parse)
        {
            if (value[0] != '[' | value[value.Length - 1] != ']')
                throw new FormatException(string.Format("{0}  is not valid format for type {1}", value, typeof(T[]).ToString()));

            string tmpStr = value.Substring(1, value.Length - 2).Trim();

            string[] items = tmpStr.Split(new char[] { ';' });
            var values = items.Select(s => parse(s.Trim()));
            return values.ToArray();
        }
    }

    public static class FuncGenerator<T>
    {
        /// <summary>
        /// Convert BinaryExpression into Func which acts as operator on types T and T. T = T op T, where op is provided by f param.
        /// </summary>
        /// <param name="f">Func which provides BinaryExpression.</param>
        /// <returns>Func&LT;T, T, T&GT; </returns>
        private static Func<T, T, T> ExpressionToFunc(Func<ParameterExpression, ParameterExpression, BinaryExpression> f)
        {
            ParameterExpression ap = Expression.Parameter(typeof(T), "a");
            ParameterExpression bp = Expression.Parameter(typeof(T), "b");
            Expression operationResult = f(ap, bp);

            Expression<Func<T, T, T>> lambda = Expression.Lambda<Func<T, T, T>>(operationResult, ap, bp);
            return lambda.Compile();
        }

        /// <summary>
        /// Convert BinaryExpression into Func which acts as operator on types T[] and T[]. T[] = T[] op T[], where op is provided by f param. 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private static Func<T[], T[], T[]> ArrayExpressionToFunc(Func<IndexExpression, IndexExpression, BinaryExpression> f)
        {
            //a, b are input parametres for returned Func<T[], T[], T[]> delegate
            //c is output (result)
            //i is loop variable
            //
            // //implementation looks like:
            // for(int i = a.Length; i > -1; i--)
            //     c[i] = a[i] op b[i];
            // return c;
            //
            ParameterExpression apA = Expression.Parameter(typeof(T[]), "a");
            ParameterExpression bpA = Expression.Parameter(typeof(T[]), "b");
            ParameterExpression operationResult = Expression.Parameter(typeof(T[]), "c");
            ParameterExpression iA = Expression.Parameter(typeof(int), "i");

            LabelTarget labelReturn = Expression.Label(typeof(T[]));

            //this block represent block inside loop
            Expression innerBlock = Expression.Block(
                Expression.SubtractAssign(iA, Expression.Constant(1)),
                Expression.IfThen(Expression.Equal(iA, Expression.Constant(-1)),
                Expression.Return(labelReturn, operationResult)),
                Expression.Assign(Expression.ArrayAccess(operationResult, iA), f(Expression.ArrayAccess(apA, iA), Expression.ArrayAccess(bpA, iA)))
                );

            //expression for easy implementation of new T[i] constructor
            Expression<Func<int, T[]>> newTA = (i) => new T[i];

            //main body of Func. Variable initialization and loop execution
            Expression addeA = Expression.Block(
                new[] { iA, operationResult },
                Expression.Assign(iA, Expression.ArrayLength(apA)),
                //Expression.Assign(cpA, Expression.NewArrayInit(typeof(T), Expression.Constant(0d), Expression.Constant(0d), Expression.Constant(0d))),
                Expression.Assign(operationResult, Expression.Invoke(newTA, iA)),
                Expression.Loop(innerBlock, labelReturn)
                );

            //Compilation to get result.
            Expression<Func<T[], T[], T[]>> lambdaA = Expression.Lambda<Func<T[], T[], T[]>>(addeA, apA, bpA);
            return lambdaA.Compile();
        }

        /// <summary>
        /// Create T + T delegate (Func). Implementation needs + operator implemented on type T.
        /// </summary>
        /// <returns>Func for T = T + T evaluation.</returns>
        public static Func<T, T, T> CreateOperatorFuncAdd()
        {
            return ExpressionToFunc((a, b) => Expression.Add(a, b));
        }

        /// <summary>
        /// Create T[] + T[] delegate (Func). Implementation needs + operator implemented on type T. + operator on type T[] does not need to be implemented.
        /// </summary>
        /// <returns>Func for T[] = T[] + T[] evaluation.</returns>
        public static Func<T[], T[], T[]> CreateArrayOperatorFuncAdd()
        {
            return ArrayExpressionToFunc((a, b) => Expression.Add(a, b));
        }
    }

}