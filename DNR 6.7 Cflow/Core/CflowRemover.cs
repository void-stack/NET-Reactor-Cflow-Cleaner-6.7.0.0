using System.Linq;
using de4dot.blocks;
using de4dot.blocks.cflow;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DNR.Core
{
    // Credits: https://github.com/SychicBoy/.NetReactorCfCleaner This is just my recoded updated version
    internal static class CflowRemover
    {
        public static uint FixedMethods { get; private set; }

        public static void Execute(Context ctx)
        {
            SimplifyArithmetic(ctx);

            var CfDeob = new BlocksCflowDeobfuscator();

            foreach (var TypeDef in ctx.Module.Types.ToArray())
                foreach (var MethodDef in TypeDef.Methods.Where(x => x.HasBody && ContainsSwitch(x)).ToArray())
                    try
                    {
                        var blocks = new Blocks(MethodDef);
                        blocks.MethodBlocks.GetAllBlocks();
                        blocks.RemoveDeadBlocks();
                        blocks.RepartitionBlocks();
                        blocks.UpdateBlocks();
                        blocks.Method.Body.SimplifyBranches();
                        blocks.Method.Body.OptimizeBranches();
                        CfDeob.Initialize(blocks);
                        CfDeob.Deobfuscate();
                        blocks.RepartitionBlocks();
                        blocks.GetCode(out
                        var instructions, out
                        var exceptionHandlers);
                        DotNetUtils.RestoreBody(MethodDef, instructions, exceptionHandlers);
                    }
                    catch
                    {
                        // ignored
                    }
        }

        private static void SimplifyArithmetic(Context ctx)
        {
            var logger = ctx.Options.Logger;

            foreach (var type in ctx.Module.Types)
                foreach (var method in type.Methods.Where(x => x.HasBody && x.Body.HasInstructions && x.Body.Instructions.Count >= 3))
                {
                    var Instructions = method.Body.Instructions;

                    for (var i = 1; i < Instructions.Count; i++)
                    {
                        if (i + 1 >= Instructions.Count) continue;

                        var prevInstr = Instructions[i - 1];
                        var curInstr = Instructions[i];
                        var nextInstr = Instructions[i + 1];

                        // Too lazy to strip these two into one function, mess
                        #region Ldsfld
                        if (curInstr.OpCode == OpCodes.Brtrue &&
                            nextInstr.OpCode == OpCodes.Pop && 
                            prevInstr.OpCode == OpCodes.Ldsfld) {
                            logger.Info($"Brtrue:  {method.FullName}");
                            prevInstr.OpCode = OpCodes.Nop;
                            curInstr.OpCode = OpCodes.Br_S;
                            FixedMethods++;
                        }
                        else if (curInstr.OpCode == OpCodes.Brfalse && 
                            nextInstr.OpCode == OpCodes.Pop &&
                            prevInstr.OpCode == OpCodes.Ldsfld) {
                            logger.Info($"Brfalse: {method.FullName}");
                            prevInstr.OpCode = OpCodes.Nop;
                            curInstr.OpCode = OpCodes.Br_S;
                            FixedMethods++;
                        }
                        #endregion

                        #region Call
                        if (curInstr.OpCode == OpCodes.Brtrue && 
                            nextInstr.OpCode == OpCodes.Pop &&
                            prevInstr.OpCode == OpCodes.Call) {
                            if (prevInstr.Operand.ToString().Contains("System.Boolean")) {
                                logger.Info($"Call with Boolean: {method.FullName}");
                                prevInstr.OpCode = OpCodes.Nop;
                                curInstr.OpCode = OpCodes.Br_S;
                            }
                            else {
                                logger.Info($"Call:  {method.FullName}");
                                prevInstr.OpCode = OpCodes.Nop;
                                curInstr.OpCode = OpCodes.Nop;
                            }
                        }
                        else if (curInstr.OpCode == OpCodes.Brfalse && 
                            nextInstr.OpCode == OpCodes.Pop &&
                            prevInstr.OpCode == OpCodes.Call) {
                             if (prevInstr.Operand.ToString().Contains("System.Boolean")) {
                                logger.Info($"Call with Boolean: {method.FullName}");
                                prevInstr.OpCode = OpCodes.Nop;
                                curInstr.OpCode = OpCodes.Nop;
                             }
                             else {
                                logger.Info($"Call: {method.FullName}");
                                prevInstr.OpCode = OpCodes.Nop;
                                curInstr.OpCode = OpCodes.Br_S;
                             }
                        }
                        #endregion
                    }
                }
        }

        private static bool ContainsSwitch(MethodDef method) => method.Body.Instructions.Any(t => t.OpCode == OpCodes.Switch);
    }
}