using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace DNR.Core
{
    public class Context
    {
        public CtxOptions Options { get; }
        public ModuleDefMD Module { get; }

        public Context(CtxOptions ctxOptions)
        {
            Options = ctxOptions;
            Module = ModuleDefMD.Load(Options.FilePath);
        }

        public void Save()
        {
            var writerOptions = new ModuleWriterOptions(Module) {
                Logger = DummyLogger.NoThrowInstance,
                MetadataOptions = {
                    Flags = MetadataFlags.PreserveAll & MetadataFlags.KeepOldMaxStack
                }
            };

            Module.Write(Options.OutputPath, writerOptions);
        }
    }
}