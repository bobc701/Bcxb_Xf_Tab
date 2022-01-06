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
        public ObservableCollection<CBatBoxSet> BatterBox { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBox { get; set; } = new();

        private CBatBoxSet _bsTot = new() { boxName = "Total" };

      //public CBatBoxSet BatterBoxVis_tot { get; set; }
      //public CBatBoxSet BatterBoxHome_tot { get; set; }
      

      public BoxScoreListViewModel(string teamName)
      {
      // -------------------------------------------------------------
      // This version of the constructor just build a dummy with
      // 9 batters and 1 pitcher and all 0's, for initial data binding
      // before a game starts.
      // -------------------------------------------------------------

         CBatBoxSet bs;
         for (int i = 1; i <= 9; i++) {
            bs = new() { boxName = $"{teamName} batter {i}", bx = i };
            BatterBox.Add(bs);
         }
         BatterBox.Add(_bsTot); //This is the totals row for vis team.

         CPitBoxSet ps;
         ps = new() { boxName = $"{teamName} pitcher 1", px = 1 };
         PitcherBox.Add(ps);

         TeamName = teamName;
      }

      public void Rebuild(CGame g, int side = 0) 
      {
         mGame = g;
         CBatter bat;
         CPitcher pit;
         int bx, px;

         TeamName = mGame.t[side].nick;

         // Visitor batter box...
         _bsTot.Zero();
         BatterBox.Clear();
         for (int i = 1; i <= CGame.SZ_BAT-1; i++) {
            bx = g.t[side].xbox[i];
            if (bx == 0) break;
            bat = g.t[side].bat[bx];
            BatterBox.Add(bat.bs);
            _bsTot.AddTo(bat.bs); 
         }
         BatterBox.Add(_bsTot); //This is the totals row.

         // Visitor pitcher box...
         PitcherBox.Clear();
         for (int i = 1; i <= CGame.SZ_PIT-1; i++) {
            px = g.t[side].ybox[i];
            if (px == 0) break;
            pit = g.t[side].pit[px];
            pit.ps.boxName = pit.pname2;
            PitcherBox.Add(pit.ps);
         }

      }


      public event PropertyChangedEventHandler PropertyChanged;
      void OnPropertyChanged([CallerMemberName] string propertyName = "") {

         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

   }
}
