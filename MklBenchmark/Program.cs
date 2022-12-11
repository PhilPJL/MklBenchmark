using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using CommunityToolkit.HighPerformance.Buffers;
using System.Numerics;

namespace MklBenchmark;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<MKLTest>();

        /*        MKLTest.BenchmarkSetup();

                var test = new MKLTest();

                var r1 = test.DotNetMultiplyComplex().ToArray();
                var r2 = test.MKLMultiplyComplex(VmlAccuracy.High).ToArray();
                var r3 = test.MKLMultiplyComplex(VmlAccuracy.Performance).ToArray();

                var ms = r1.Zip(r2, r3)
                    .Select(z => new { r1 = z.First, r2 = z.Second, r3 = z.Third })
                    .Select(z => new { m1 = z.r2 - z.r1, m2 = z.r3 - z.r1 });

                foreach(var m in ms.Where(m1 => m1.m1 != 0 || m1.m2 != 0))
                {
                    Console.WriteLine($"{m.m1}, {m.m2}");
                }

                Console.WriteLine(ms.Where(m1 => m1.m1 != 0 || m1.m2 != 0).Count());
                Console.WriteLine(ms.Where(m1 => m1.m1 != 0).Count());
                Console.WriteLine(ms.Where(m1 => m1.m2 != 0).Count());*/
    }
}

public class MKLTest : ManualConfig
{
    const int Size = 40000;

    [Benchmark]
    public void DotNetASin()
    {
        using var owner = SpanOwner<double>.Allocate(Size);

        for (int i = 0; i < Size; i++)
        {
            owner.Span[i] = Math.Asin(Values1[i]);
        }
    }

    [Benchmark]
    public void MKLASin()
    {
        using var owner1 = SpanOwner<double>.Allocate(Size);

        MKLNativeMethods.Asin(Values1, owner1.Span);
    }

    [Benchmark]
    public void DotNetCos()
    {
        using var owner = SpanOwner<double>.Allocate(Size);

        for (int i = 0; i < Size; i++)
        {
            owner.Span[i] = Math.Cos(Values2[i]);
        }
    }

    [Benchmark]
    public void MKLCos()
    {
        using var owner1 = SpanOwner<double>.Allocate(Size);

        MKLNativeMethods.Cos(Values2, owner1.Span);
    }

    [Benchmark]
    public void DotNetASinCos()
    {
        using var owner = SpanOwner<double>.Allocate(Size);

        for (int i = 0; i < Size; i++)
        {
            owner.Span[i] = Math.Cos(Math.Asin(Values1[i]));
        }
    }

    [Benchmark]
    public void MKLASinCos()
    {
        using var owner1 = SpanOwner<double>.Allocate(Size);
        using var owner2 = SpanOwner<double>.Allocate(Size);

        MKLNativeMethods.Asin(Values1, owner1.Span);
        MKLNativeMethods.Cos(owner1.Span, owner2.Span);
    }


    [Benchmark]
    public Span<Complex> DotNetMultiplyComplex()
    {
        using var spanOwner = SpanOwner<Complex>.Allocate(Size);
        var result = spanOwner.Span;

        for (int i = 0; i < Size; i++)
        {
            result[i] = CValues1[i] * CValues2[i];
        }

        return result;
    }

    [Benchmark]
    [Arguments(VmlAccuracy.Low)]
    [Arguments(VmlAccuracy.High)]
    [Arguments(VmlAccuracy.Performance)]
    public Span<Complex> MKLMultiplyComplex(VmlAccuracy accuracy)
    {
        using var spanOwner = SpanOwner<Complex>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Multiply(CValues1, CValues2, result, accuracy);

        return result;
    }

    [Benchmark]
    public void MKLMultiplyNativeComplex()
    {
        using var spanOwner = SpanOwner<MKLComplex16>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Multiply(MKLCValues1, MKLCValues2, result);
    }

    [Benchmark]
    public void DotNetMultiplyDouble()
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        for (int i = 0; i < Size; i++)
        {
            result[i] = Values1[i] * Values2[i];
        }
    }

    [Benchmark]
    [Arguments(VmlAccuracy.Low)]
    [Arguments(VmlAccuracy.High)]
    [Arguments(VmlAccuracy.Performance)]
    public void MKLMultiplyDouble(VmlAccuracy accuracy)
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Multiply(Values1, Values2, result, accuracy);
    }

    [GlobalSetup]
    public static void BenchmarkSetup()
    {
        Random r = new Random(Environment.TickCount);

        for (int i = 0; i < Size; i++)
        {
            Values1[i] = Math.Sin(Math.PI * 2 * Size / i); // setup with values that work with Asin
            Values2[i] = Math.PI * 2 * Size / i;
            CValues1[i] = new Complex(r.NextDouble(), r.NextDouble());
            CValues2[i] = new Complex(r.NextDouble(), r.NextDouble());
            MKLCValues1[i] = new MKLComplex16 { Real = r.NextDouble(), Imag = r.NextDouble() };
            MKLCValues2[i] = new MKLComplex16 { Real = r.NextDouble(), Imag = r.NextDouble() };
        }
    }

    public static readonly double[] Values1 = new double[Size];
    public static readonly double[] Values2 = new double[Size];
    public static readonly Complex[] CValues1 = new Complex[Size];
    public static readonly Complex[] CValues2 = new Complex[Size];
    public static readonly MKLComplex16[] MKLCValues1 = new MKLComplex16[Size];
    public static readonly MKLComplex16[] MKLCValues2 = new MKLComplex16[Size];
}
