using CommunityToolkit.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MklBenchmark;

public unsafe static partial class MKLNativeMethods
{
    #region Cos

    public static void Cos(ReadOnlySpan<double> a, Span<double> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vdCos(a.Length, a, r);
#else
        fixed (double* pA = a)
        fixed (double* pR = r)
        {
            vdCos(a.Length, pA, pR);
        }
#endif  
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Cos(ReadOnlySpan<Complex> a, Span<Complex> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        var a1 = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var r1 = MemoryMarshal.Cast<Complex, MKLComplex16>(r);
        vzCos(a.Length, a1, r1);
#else
        fixed (Complex* pA = a)
        fixed (Complex* pR = r)
        {
            vzCos(a.Length, pA, pR);
        }
#endif
    }

#if NET7_0_OR_GREATER
    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vdCos(int n, ReadOnlySpan<double> a, Span<double> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vzCos(int n, ReadOnlySpan<MKLComplex16> a, Span<MKLComplex16> r);
#else
    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public unsafe static extern void vdCos(int n, double* a, double* r);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public unsafe static extern void vzCos(int n, Complex* a, Complex* r);
#endif

    #endregion

    #region ASin

    public static void Asin(ReadOnlySpan<double> a, Span<double> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vdAsin(a.Length, a, r);
#else
        fixed (double* pA = a)
        fixed (double* pR = r)
        {
            vdAsin(a.Length, pA, pR);
        }
#endif
    }

    public static void Asin(ReadOnlySpan<Complex> a, Span<Complex> r)
    {
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        var a1 = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var r1 = MemoryMarshal.Cast<Complex, MKLComplex16>(r);
        vzAsin(a.Length, a1, r1);
#else
        fixed (Complex* pA = a)
        fixed (Complex* pR = r)
        {
            vzAsin(a.Length, pA, pR);
        }
#endif
    }

#if NET7_0_OR_GREATER
    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vdAsin(int n, ReadOnlySpan<double> a, Span<double> r);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void vzAsin(int n, ReadOnlySpan<MKLComplex16> a, Span<MKLComplex16> r);
#else
    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public unsafe static extern void vdAsin(int n, double* a, double* r);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public unsafe static extern void vzAsin(int n, Complex* a, Complex* r);
#endif


    #endregion

    #region Multiply

    public static void Multiply(ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        var lhs = MemoryMarshal.Cast<Complex, MKLComplex16>(a);
        var rhs = MemoryMarshal.Cast<Complex, MKLComplex16>(b);
        var res = MemoryMarshal.Cast<Complex, MKLComplex16>(r);

        vmzMul(lhs.Length, lhs, rhs, res, accuracy);
#else
        fixed (Complex* pA = a)
        fixed (Complex* pB = b)
        fixed (Complex* pR = r)
        {
            vmzMul(a.Length, pA, pB, pR, accuracy);
        }
#endif
    }

    public static void Multiply(ReadOnlySpan<MKLComplex16> a, ReadOnlySpan<MKLComplex16> b, Span<MKLComplex16> r)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vzMul(a.Length, a, b, r);
#else
        fixed (MKLComplex16* pA = a)
        fixed (MKLComplex16* pB = b)
        fixed (MKLComplex16* pR = r)
        {
            vzMul(a.Length, pA, pB, pR);
        }
#endif
    }

    public static void Multiply(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vmdMul(a.Length, a, b, r, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdMul(a.Length, pA, pB, pR, accuracy);
        }
#endif
    }

    public static void Multiply(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, int increment, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        int len = (a.Length + 1) / increment;

#if NET7_0_OR_GREATER
        vmdMulI(a.Length, a, increment, b, increment, r, increment, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdMulI(a.Length, pA, increment, pB, increment, pR, increment, accuracy);
        }
#endif
    }

#if NET7_0_OR_GREATER
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

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdMulI(int n, ReadOnlySpan<double> a, int inca, ReadOnlySpan<double> b, int incb, Span<double> r, int incr, VmlAccuracy mode);
#else
    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vzMul(int n, Complex* a, Complex* b, Complex* r);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmzMul(int n, Complex* a, Complex* b, Complex* r, VmlAccuracy mode);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vzMul(int n, MKLComplex16* a, MKLComplex16* b, MKLComplex16* r);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vdMul(int n, double* a, double* b, double* r);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdMul(int n, double* a, double* b, double* r, VmlAccuracy mode);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdMulI(int n, double* a, int inca, double* b, int incb, double* r, int incr, VmlAccuracy mode);
#endif

    #endregion

    // TODO: add Div/Sub/Add/Exp/Pow/Sin... etc

    #region Add
    public static void Add(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vmdAdd(a.Length, a, b, r, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdAdd(a.Length, pA, pB, pR, accuracy);
        }
#endif
    }

    public static void Add(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, int increment, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

        int len = (a.Length + 1) / increment;

#if NET7_0_OR_GREATER
        vmdAddI(len, a, increment, b, increment, r, increment, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdAddI(a.Length, pA, increment, pB, increment, pR, increment, accuracy);
        }
#endif
    }

#if NET7_0_OR_GREATER
    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdAdd(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy mode);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdAddI(int n, ReadOnlySpan<double> a, int inca, ReadOnlySpan<double> b, int incb, Span<double> y, int incy, VmlAccuracy mode);
#else
    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdAdd(int n, double* a, double* b, double* r, VmlAccuracy mode);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdAddI(int n, double* a, int inca, double* b, int incb, double* y, int incy, VmlAccuracy mode);
#endif
    #endregion

    #region Subtract
    public static void Subtract(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);

#if NET7_0_OR_GREATER
        vmdSub(a.Length, a, b, r, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdSub(a.Length, pA, pB, pR, accuracy);
        }
#endif
    }

    public static void Subtract(ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, int increment, VmlAccuracy accuracy = VmlAccuracy.High)
    {
        Guard.IsGreaterThanOrEqualTo(b.Length, a.Length);
        Guard.IsGreaterThanOrEqualTo(r.Length, a.Length);
        
        int len = (a.Length + 1) / increment;

#if NET7_0_OR_GREATER
        vmdSubI(len, a, increment, b, increment, r, increment, accuracy);
#else
        fixed (double* pA = a)
        fixed (double* pB = b)
        fixed (double* pR = r)
        {
            vmdSubI(a.Length, pA, increment, pB, increment, pR, increment, accuracy);
        }
#endif
    }

#if NET7_0_OR_GREATER
    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdSub(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> r, VmlAccuracy mode);

    [LibraryImport("mkl_rt.2.dll")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void vmdSubI(int n, ReadOnlySpan<double> a, int inca, ReadOnlySpan<double> b, int incb, Span<double> r, int incr, VmlAccuracy mode);
#else
    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdSub(int n, double* a, double* b, double* r, VmlAccuracy mode);

    [DllImport("mkl_rt.2.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void vmdSubI(int n, double* a, int inca, double* b, int incb, double* r, int incr, VmlAccuracy mode);
#endif
    #endregion
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