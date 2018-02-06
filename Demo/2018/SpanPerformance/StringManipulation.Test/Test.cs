using Xunit;

namespace StringManipulation
{
    public class StringManipulationTest
    {
        IStringManipulater[] targets = new IStringManipulater[]
        {
            new Classic.Manipulater(),
            new Unsafe.Manipulater(),
            new SafeStackalloc.Manipulater(),
            new FullyTuned.Manipulater(),
        };

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("DeriveRestrictedAppContainerSidFromAppContainerSidAndRestrictedName")]
        [InlineData("InternalFrameInternalFrameTitlePaneInternalFrameTitlePaneMaximizeButtonWindowNotFocusedState")]
        public void CamelToSnakeToCamel(string s)
        {
            foreach (var t in targets) AssertCamelToSnakeToCamel(s, t);
        }

        void AssertCamelToSnakeToCamel(string s, IStringManipulater m)
        {
            var snake = m.CamelToSnake(s);
            var camel = m.SnakeToCamel(snake);
            Assert.Equal(s, camel);
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("a_b_c_d_e_f_g_h_i_j_k_l_m_n_o_p_q_r_s_t_u_v_w_x_y_z")]
        [InlineData("ensure_name_not_reserved")]
        public void SnakeToCamelToSnake(string s)
        {
            foreach (var t in targets) AssertSnakeToCamelToSnake(s, t);
        }

        void AssertSnakeToCamelToSnake(string s, IStringManipulater m)
        {
            var camel = m.SnakeToCamel(s);
            var snake = m.CamelToSnake(camel);
            Assert.Equal(s, snake);
        }
    }
}
