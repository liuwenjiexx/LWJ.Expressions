/**************************************************************
 *  Filename:    InvocationContext.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Expressions
{

    public class InvocationContext
    {

        private InvocationContext parent;
        private IExpressionContext context;
        private GotoType gotoType;
        private BlockType blockType;
        private object gotoValue;

        private static InvocationContext current;

        private static object lockObj = new object();

        internal InvocationContext()
            : this(null)
        {

        }

        internal InvocationContext(IExpressionContext context)
            : this(null, context, false)
        {

        }



        private InvocationContext(InvocationContext parent, IExpressionContext context, bool isDefault)
        {

            if (context == null)
            {
                if (parent == null)
                    context = new ExpressionContext();
                else
                    context = new ExpressionContext(null, parent.Context);
            }
            this.context = context;
             
             
            if (parent == null)
            {
                if (isDefault)
                    InitDefault();
                else
                    parent = Default;
            }
            else
            {
                this.parent = parent;
            }
        }

        private static InvocationContext Default { get => null; }

        private InvocationContext Parent => parent;

        public GotoType GotoType { get => gotoType; }
        public object GotoValue { get => gotoValue; }
        public BlockType BlockType { get => blockType; }

        public IExpressionContext Context { get => context; }

        private void InitDefault()
        {

        }


        #region Variable


        public bool ContainsVariable(string name)
        {
            return context.ContainsVariable(name);
        }

        public void SetVariable(string name, object value)
        {
            context.SetVariable(name, value);
        }

        public Type GetVariableType(string name)
        {
            return context.GetVariableType(name);
        }

        public object GetVariable(string name)
        {
            return context.GetVariable(name);
        }
        #endregion


        public void BubblingGoto()
        {
            if (parent != null)
            {
                parent.SetGoto(gotoType, gotoValue);
            }
        }

        private InvocationContext FindGotoScope(InvocationContext parent)
        {
            var p = parent;
            while (p != null && p.BlockType != BlockType.Block)
                p = p.parent;
            return p;
        }

        public void HandleGoto()
        {
            gotoType = GotoType.None;
        }

        public void SetGoto(GotoType gotoType, object gotoValue)
        {
            this.gotoType = gotoType;
            this.gotoValue = gotoValue;
        }

        public InvocationContext CreateChild(BlockType scopeType = BlockType.Block)
        {
            return CreateChild(null, scopeType);
        }

        public InvocationContext CreateChild(IExpressionContext context, BlockType blockType = BlockType.Block)
        {
            if (context == null)
                context = new ExpressionContext(null, this.context);

            var child = new InvocationContext(this, context, false);
            child.blockType = blockType;
            child.gotoType = GotoType.None;
            return child;
        }


    }


    internal class OperatorInfo
    {
        public Type operType1;
        public Type operType2;
        public Type resultType;
        public bool inverse;
        public ExpressionType expressionType;
        public MethodInfo method;

        public OperatorInfo(ExpressionType exprType, Type operType1, Type operType2, Type result)
        {
            this.expressionType = exprType;
            this.operType1 = operType1;
            this.operType2 = operType2;
            this.resultType = result;
        }


    }

}