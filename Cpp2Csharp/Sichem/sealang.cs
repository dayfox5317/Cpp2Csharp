using ClangSharp.Interop;
using System.Runtime.InteropServices;

namespace SealangSharp
{
    public static class sealang
    {
        public const string libraryPath = "sealang";

        [DllImport("sealang", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sealang_Cursor_getOperatorString")]
        public static extern CXString cursor_getOperatorString(CXCursor cursor);

        [DllImport("sealang", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sealang_Cursor_getBinaryOpcode")]
        public static extern BinaryOperatorKind cursor_getBinaryOpcode(CXCursor cursor);

        [DllImport("sealang", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sealang_Cursor_getUnaryOpcode")]
        public static extern UnaryOperatorKind cursor_getUnaryOpcode(CXCursor cursor);

        [DllImport("sealang", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sealang_Cursor_getLiteralString")]
        public static extern CXString cursor_getLiteralString(CXCursor cursor);
    }
    public enum BinaryOperatorKind
    {
        PtrMemD,
        PtrMemI,
        Mul,
        Div,
        Rem,
        Add,
        Sub,
        Shl,
        Shr,
        ThreeWayComparison,
        LT,
        GT,
        LE,
        GE,
        EQ,
        NE,
        And,
        Xor,
        Or,
        LAnd,
        LOr,
        Assign,
        MulAssign,
        DivAssign,
        RemAssign,
        AddAssign,
        SubAssign,
        ShlAssign,
        ShrAssign,
        AndAssign,
        XorAssign,
        OrAssign,
        Comma
    }
    public enum UnaryOperatorKind
    {
        PostInc,
        PostDec,
        PreInc,
        PreDec,
        AddrOf,
        Deref,
        Plus,
        Minus,
        Not,
        LNot,
        Real,
        Imag,
        Extension,
        Coawait
    }

}
