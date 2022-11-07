using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[DisassemblyDiagnoser(exportDiff: true, exportCombinedDisassemblyReport: true)]
public class Bench
{
    [ModuleInitializer]
    public static void RunCctor()
    {
        RuntimeHelpers.RunClassConstructor(typeof(Bench).TypeHandle);
    }

    private const int Size = 10_000_000;
    
    private static readonly byte[] Source, Dest;
    
    static Bench()
    {
        Source = GC.AllocateUninitializedArray<byte>(Size);
        
        Dest = GC.AllocateUninitializedArray<byte>(Size);
    }
    
    [Benchmark]
    public byte[] ToArray()
    {
        return Source.ToArray();
    }
    
    [Benchmark]
    public byte[] ArrayCopy()
    {
        Array.Copy(Source, Dest, Size);
        
        return Dest;
    }
    
    [Benchmark]
    public byte[] SpanCopyTo()
    {
        Source.AsSpan().CopyTo(Dest);
        
        return Dest;
    }
    
    [Benchmark]
    public byte[] SpanCopyToNoChecks()
    {
        var SourceSpan = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(Source), Size);
        
        var DestSpan = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(Dest), Size);
        
        SourceSpan.CopyTo(DestSpan);
        
        return Dest;
    }
}