using CommunityToolkit.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MklBenchmark;

public static partial class MKLNativeMethods
{
    #region Cos

    public static void Cos(double[] a, double[] r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        // Implicit conversion to Span
        vdCos(a.Length, a, r);
    }

    public static void Cos(ReadOnlySpan<double> a, Span<double> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        vdCos(a.Length, a, r);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Cos(ReadOnlySpan<Complex> a, Span<Complex> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        var a1 = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var r1 = MemoryMarshal.Cast<Complex, MKLComplex16>(r);
        vzCos(a.Length, a1, r1);
    }

    // TODO: add vsCos if required

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vdCos(int n, ReadOnlySpan<double> a, Span<double> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vzCos(int n, ReadOnlySpan<MKLComplex16> a, Span<MKLComplex16> r);

    #endregion

    #region ASin

    public static void Asin(double[] a, double[] r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        vdAsin(a.Length, a, r);
    }

    public static void Asin(ReadOnlySpan<double> a, Span<double> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        vdAsin(a.Length, a, r);
    }

    public static void Asin(ReadOnlySpan<Complex> a, Span<Complex> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        var a1 = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var r1 = MemoryMarshal.Cast<Complex, MKLComplex16>(r);
        vzAsin(a.Length, a1, r1);
    }

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vdAsin(int n, ReadOnlySpan<double> a, Span<double> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vzAsin(int n, ReadOnlySpan<MKLComplex16> a, Span<MKLComplex16> r);

    #endregion

    #region Multiply

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        var lhs = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var rhs = MemoryMarshal.Cast<Complex, MKLComplex16>(b);
        var res = MemoryMarshal.Cast<Complex, MKLComplex16>(r);

        vmzMul(lhs.Length, lhs, rhs, res, accuracy);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ReadOnlySpan<MKLComplex16> a, ReadOnlySpan<MKLComplex16> b, Span<MKLComplex16> r)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        vzMul(a.Length, a, b, r);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        vmdMul(a.Length, a, b, r, accuracy);
    }

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vzMul(int n, ReadOnlySpan<MKLComplex16> a, ReadOnlySpan<MKLComplex16> b, Span<MKLComplex16> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmzMul(int n, ReadOnlySpan<MKLComplex16> a, ReadOnlySpan<MKLComplex16> b, Span<MKLComplex16> r, VmlAccuracy mode);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vdMul(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdMul(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy mode);

    #endregion

    // TODO: add Div/Sub/Add/Exp/Pow/Sin... etc
}

public struct MKLComplex16
{
    public double Real;
    public double Imag;
}

public enum VmlAccuracy : uint
{
    Low = 1,
    High = 2,
    Performance = 3
}