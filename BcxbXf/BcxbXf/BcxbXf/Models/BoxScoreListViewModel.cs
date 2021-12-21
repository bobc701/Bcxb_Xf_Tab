using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BCX.BCXB;

namespace BcxbXf.Models {

   class BoxScoreListViewModel : INotifyPropertyChanged { 

      private CGame mGame;

        public ObservableCollection<CBatBoxSet> BatterBoxVis { get; set; } = new();
        public ObservableCollection<CBatBoxSet> BatterBoxHome { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBoxVis { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBoxHome { get; set; } = new();

      //public CBatBoxSet BatterBoxVis_tot { get; set; }
      //public CBatBoxSet BatterBoxHome_tot { get; set; }
      
        public string VisName { get { return mGame?.t[0].nick ?? "Visitor"; } }
        public string HomeName { get { return mGame?.t[1].nick ?? "Home"; } }

      public BoxScoreListViewModel()
      {
      // -------------------------------------------------------------
      // This version of the constructor just build a dummy with
      // 9 batters and 1 pitcher and all 0's, for initial data binding
      // before a game starts.
      // -------------------------------------------------------------

         CBatBoxSet bs;
         for (int i = 1; i <= 9; i++) {
            bs = new() { boxName = $"Visitor batter {i}", bx = i };
            BatterBoxVis.Add(bs);
            //BatterBoxVis_tot = bs;
            bs = new() { boxName = $"Home batter {i}", bx = i };
            BatterBoxHome.Add(bs);
            //BatterBoxHome_tot = bs;
         }
         bs = new() { boxName = "Total" };
         BatterBoxVis.Add(bs); //This is the totals row for vis team.
         bs = new() { boxName = "Total" };
         BatterBoxVis.Add(bs); //This is the totals row for home team.

         CPitBoxSet ps;
         ps = new() { boxName = "Visitor pitcher 1", px = 1 };
         PitcherBoxVis.Add(ps);

         ps = new() { boxName = "Home pitcher 1", px = 1 };
         PitcherBoxHome.Add(ps);

      }

      public BoxScoreListViewModel(CGame g, int side = 0) 
        {
        // ------------------------------------------------------
        // This version of the constructor is for in-game and 
        // uses mGame and side.
        // ------------------------------------------------------
         mGame = g;
         CBatter bat;
         CPitcher pit;
         int bx, px;
         CBatBoxSet bsTot;

      // Visitor batter box...
         bsTot = new CBatBoxSet() { boxName = "Total"};
         for (int i = 1; i <= CGame.SZ_BAT; i++) {
            bx = g.t[0].xbox[i];
            if (bx == 0) break;
            bat = g.t[0].bat[bx];
            BatterBoxVis.Add(bat.bs);
            bsTot += bat.bs; //Operator overload! 
         }
         BatterBoxVis.Add(bsTot); //This is the totals row.

      // Home batter box...
         bsTot = new CBatBoxSet() { boxName = "Total"};
         for (int i = 1; i <= CGame.SZ_BAT; i++) {
            bx = g.t[1].xbox[i];
            if (bx == 0) break;
            bat = g.t[1].bat[bx];
            BatterBoxHome.Add(bat.bs);
            bsTot += bat.bs; //Operator overload! 
         }
         BatterBoxHome.Add(bsTot); //This is the totals row.

      // Visitor pitcher box...
         for (int i = 1; i <= CGame.SZ_PIT; i++) {
            px = g.t[0].ybox[i];
            if (px == 0) break;
            pit = g.t[0].pit[px];
            pit.ps.boxName = pit.pname2;
            PitcherBoxVis.Add(pit.ps);
         }

      // Home pitchers box...
         for (int i = 1; i <= CGame.SZ_PIT; i++) {
            px = g.t[1].xbox[i];
            if (px == 0) break;
            pit = g.t[1].pit[px];
            pit.ps.boxName = pit.pname2;
            PitcherBoxHome.Add(pit.ps);
         }

            //var bs1 = new CBatBoxSet();
            //foreach (var bs in BatterBoxVis) bsTot = bsTot + bs; //Operator overload!
            //BatterBoxVis_tot = bsTot;

      }


      public event PropertyChangedEventHandler PropertyChanged;
      void OnPropertyChanged([CallerMemberName] string propertyName = "") {

         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

   }
}
