using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BCX.BCXB;

namespace BcxbXf.Models {

   public class BoxScoreListViewModel : INotifyPropertyChanged { 

      private CGame mGame;

         public string TeamName { get; set; }
        public ObservableCollection<CBatBoxSet> BatterBoxVis { get; set; } = new();
        public ObservableCollection<CBatBoxSet> BatterBoxHome { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBoxVis { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBoxHome { get; set; } = new();

      private CBatBoxSet _bsTot = new() { boxName = "Total" };

      private ObservableCollection<CBatBoxSet> batterBox;
      private ObservableCollection<CPitBoxSet> pitcherBox;
      //public CBatBoxSet BatterBoxVis_tot { get; set; }
      //public CBatBoxSet BatterBoxHome_tot { get; set; }


      public BoxScoreListViewModel(string teamName)
      {
         TeamName = teamName;
      }

      public void Rebuild(CGame g, int side = 0) 
      {
         mGame = g;
         CBatter bat;
         CPitcher pit;
         int bx, px;
         batterBox = side switch { 0 => BatterBoxVis, 1 => BatterBoxHome };
         pitcherBox = side switch { 0 => PitcherBoxVis, 1 => PitcherBoxHome };

         TeamName = mGame.t[side].nick;

         // Batter box...
         _bsTot.Zero();
         batterBox.Clear();
         for (int i = 1; i <= CGame.SZ_BAT-1; i++) {
            bx = g.t[side].xbox[i];
            if (bx == 0) break;
            bat = g.t[side].bat[bx];
            batterBox.Add(bat.bs);
            _bsTot.AddTo(bat.bs); 
         }
         batterBox.Add(_bsTot); //This is the totals row.

         // Pitcher box...
         pitcherBox.Clear();
         for (int i = 1; i <= CGame.SZ_PIT-1; i++) {
            px = g.t[side].ybox[i];
            if (px == 0) break;
            pit = g.t[side].pit[px];
            pit.ps.boxName = pit.pname2;
            pitcherBox.Add(pit.ps);
         }

      }


      public event PropertyChangedEventHandler PropertyChanged;
      void OnPropertyChanged([CallerMemberName] string propertyName = "") {

         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

   }
}
