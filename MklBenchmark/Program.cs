using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using CommunityToolkit.HighPerformance.Buffers;
using System.Numerics;

namespace MklBenchmark;

internal class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<MKLTest>();
    }
}

public class MKLTest : ManualConfig
{
    const int Size = 400;

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
    public void DotNetMultiplyComplex()
    {
        using var spanOwner = SpanOwner<Complex>.Allocate(Size);
        var result = spanOwner.Span;

        for (int i = 0; i < Size; i++)
        {
            result[i] = CValues1[i] * CValues2[i];
        }
    }

    [Benchmark]
    [Arguments(VmlAccuracy.Low)]
    [Arguments(VmlAccuracy.High)]
    [Arguments(VmlAccuracy.Performance)]
    public void MKLMultiplyComplex(VmlAccuracy accuracy)
    {
        using var spanOwner = SpanOwner<Complex>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Multiply(CValues1, CValues2, result, accuracy);
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

    [Benchmark]
    public void DotNetAddDouble()
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        for (int i = 0; i < Size; i++)
        {
            result[i] = Values1[i] + Values2[i];
        }
    }

    [Benchmark]
    public void MKLAddDouble()
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Add(Values1, Values2, result);
    }

    [Benchmark]
    public void DotNetSubtract()
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        for (int i = 0; i < Size; i++)
        {
            result[i] = Values1[i] - Values2[i];
        }
    }

    [Benchmark]
    public void MKLSubtractDouble()
    {
        using var spanOwner = SpanOwner<double>.Allocate(Size);
        var result = spanOwner.Span;

        MKLNativeMethods.Subtract(Values1, Values2, result);
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
