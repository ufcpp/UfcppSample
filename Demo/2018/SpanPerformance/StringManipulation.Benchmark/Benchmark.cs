using BenchmarkDotNet.Attributes;
using System;

namespace StringManipulation.Benchmark
{
    public class StringManipulationBenchmark
    {
        string[] _data = new[]
        {
            "a",
            "ab",
            "abc",
            "a_b_c",
            "A",
            "AB",
            "ABC",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            "DeriveRestrictedAppContainerSidFromAppContainerSidAndRestrictedName",
            "InternalFrameInternalFrameTitlePaneInternalFrameTitlePaneMaximizeButtonWindowNotFocusedState",
            "abcdefghijklmnopqrstuvwxyz",
            "a_b_c_d_e_f_g_h_i_j_k_l_m_n_o_p_q_r_s_t_u_v_w_x_y_z",
            "ensure_name_not_reserved",
        };

        [Benchmark] public void Classic() => Run<Classic.Manipulater>();
        [Benchmark] public void Unsafe() => Run<Unsafe.Manipulater>();
        [Benchmark] public void SafeStackalloc() => Run<SafeStackalloc.Manipulater>();

        void Run<M>()
            where M : struct, IStringManipulater
        {
            foreach (var x in _data)
            {
                var s = default(M).CamelToSnake(x);
                var c = default(M).SnakeToCamel(x);
                var s1 = default(M).CamelToSnake(c);
                var c1 = default(M).SnakeToCamel(s);
            }
        }
    }
}
