using CardBattle.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardBattle.Views
{
    /// <summary>
    /// Interaction logic for PlayAreaControl.xaml
    /// </summary>
    public partial class PlayAreaControl : UserControl
    {
        public PlayAreaControl()
        {
            InitializeComponent();

            _playerAttackAnimation = CreateDamageAnimation(playerAttackPanel);
            _bossAttackAnimation = CreateDamageAnimation(bossAttackPanel);
        }

        Storyboard _playerAttackAnimation;
        Storyboard _bossAttackAnimation;

        public async Task ShowAttack(PlayerAttack attack)
        {
            playerAttackPanel.DataContext = attack;
            playerAttackPanel.Visibility = Visibility.Visible;

            await _playerAttackAnimation.PlayAsync();

            playerAttackPanel.Visibility = Visibility.Collapsed;
        }

        public async Task ShowAttack(BossAttack attack)
        {
            bossAttackPanel.DataContext = attack;
            bossAttackPanel.Visibility = Visibility.Visible;

            await _bossAttackAnimation.PlayAsync();

            bossAttackPanel.Visibility = Visibility.Collapsed;
        }

        private Storyboard CreateDamageAnimation(DependencyObject target)
        {
            var appeal = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                AutoReverse = true,
            };

            Storyboard.SetTarget(appeal, target);
            Storyboard.SetTargetProperty(appeal, new PropertyPath(OpacityProperty));

            var s = new Storyboard { Children = { appeal } };
            return s;
        }
    }
}
