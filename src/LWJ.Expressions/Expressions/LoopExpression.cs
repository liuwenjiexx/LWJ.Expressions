namespace LWJ.Expressions
{
    public class LoopExpression : Expression
    {
        private Expression body;

        public LoopExpression(Expression body)
            : base(ExpressionType.Loop)
        {
            this.body = body;
        }

        public Expression Body => body;

        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var bodyEval = body.Compile(ctx);

            return (invoke) =>
            {
                InvocationContext child;
                while (true)
                {
                    child = invoke.CreateChild(BlockType.Loop);

                    bodyEval(child);
                    if (child.GotoType != GotoType.None)
                    {
                        if (child.GotoType == GotoType.Break)
                        {
                            child.HandleGoto();
                            break;
                        }
                        if (child.GotoType == GotoType.Continue)
                        {
                            child.HandleGoto();
                            continue;
                        }
                        if (child.GotoType == GotoType.Return)
                        {
                            child.BubblingGoto();
                            break;
                        }

                    }
                }
                return null;
            };
        }
    }



}
