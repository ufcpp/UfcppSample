using System.Numerics;

namespace Algebra.Constants;

public struct RationalConstant<TBase, N> : IConstant<Rational<TBase>>
    where TBase : IAdditiveIdentity<TBase, TBase>,
    IMultiplicativeIdentity<TBase, TBase>,
    IAdditionOperators<TBase, TBase, TBase>,
    ISubtractionOperators<TBase, TBase, TBase>,
    IMultiplyOperators<TBase, TBase, TBase>,
    IDivisionOperators<TBase, TBase, TBase>,
    IUnaryNegationOperators<TBase, TBase>,
    IComparisonOperators<TBase, TBase>
    where N : IConstant<TBase>
{
    public static Rational<TBase> Value { get; } = new(N.Value);
}

public static class RationalConstants<T>
    where T : INumber<T>
{
    public struct _0 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(0); }
    public struct _1 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(1); }
    public struct _2 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(2); }
    public struct _3 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(3); }
    public struct _4 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(4); }
    public struct _5 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(5); }
    public struct _6 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(6); }
    public struct _7 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(7); }
    public struct _8 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(8); }
    public struct _9 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(9); }
    public struct _10 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(10); }
    public struct _11 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(11); }
    public struct _12 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(12); }
    public struct _13 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(13); }
    public struct _14 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(14); }
    public struct _15 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(15); }
    public struct _16 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(16); }
    public struct _17 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(17); }
    public struct _18 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(18); }
    public struct _19 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(19); }
    public struct _20 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(20); }
    public struct _21 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(21); }
    public struct _22 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(22); }
    public struct _23 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(23); }
    public struct _24 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(24); }
    public struct _25 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(25); }
    public struct _26 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(26); }
    public struct _27 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(27); }
    public struct _28 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(28); }
    public struct _29 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(29); }
    public struct _30 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(30); }
    public struct _31 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(31); }
    public struct _32 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(32); }
    public struct _33 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(33); }
    public struct _34 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(34); }
    public struct _35 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(35); }
    public struct _36 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(36); }
    public struct _37 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(37); }
    public struct _38 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(38); }
    public struct _39 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(39); }
    public struct _40 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(40); }
    public struct _41 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(41); }
    public struct _42 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(42); }
    public struct _43 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(43); }
    public struct _44 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(44); }
    public struct _45 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(45); }
    public struct _46 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(46); }
    public struct _47 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(47); }
    public struct _48 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(48); }
    public struct _49 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(49); }
    public struct _50 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(50); }
    public struct _51 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(51); }
    public struct _52 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(52); }
    public struct _53 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(53); }
    public struct _54 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(54); }
    public struct _55 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(55); }
    public struct _56 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(56); }
    public struct _57 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(57); }
    public struct _58 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(58); }
    public struct _59 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(59); }
    public struct _60 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(60); }
    public struct _61 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(61); }
    public struct _62 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(62); }
    public struct _63 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(63); }
    public struct _64 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(64); }
    public struct _65 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(65); }
    public struct _66 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(66); }
    public struct _67 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(67); }
    public struct _68 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(68); }
    public struct _69 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(69); }
    public struct _70 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(70); }
    public struct _71 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(71); }
    public struct _72 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(72); }
    public struct _73 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(73); }
    public struct _74 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(74); }
    public struct _75 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(75); }
    public struct _76 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(76); }
    public struct _77 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(77); }
    public struct _78 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(78); }
    public struct _79 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(79); }
    public struct _80 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(80); }
    public struct _81 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(81); }
    public struct _82 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(82); }
    public struct _83 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(83); }
    public struct _84 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(84); }
    public struct _85 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(85); }
    public struct _86 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(86); }
    public struct _87 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(87); }
    public struct _88 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(88); }
    public struct _89 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(89); }
    public struct _90 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(90); }
    public struct _91 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(91); }
    public struct _92 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(92); }
    public struct _93 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(93); }
    public struct _94 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(94); }
    public struct _95 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(95); }
    public struct _96 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(96); }
    public struct _97 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(97); }
    public struct _98 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(98); }
    public struct _99 : IConstant<Rational<T>> { public static Rational<T> Value { get; } = T.CreateChecked(99); }

    public struct M1 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-1); }
    public struct M2 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-2); }
    public struct M3 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-3); }
    public struct M4 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-4); }
    public struct M5 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-5); }
    public struct M6 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-6); }
    public struct M7 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-7); }
    public struct M8 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-8); }
    public struct M9 : IConstant<Rational<T>> { public static Rational<T> Value => T.CreateChecked(-9); }
}
