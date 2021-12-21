using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BCX.BCXB;

namespace BcxbXf.Models {

   class BoxScoreListViewModel : INotifyPropertyChanged { 

      public ObservableCollection<CBatBoxSet> BatterBoxVis { get; set; }
      //public ObservableCollection<CBatBoxSet> BatterBoxHome { get; set; }
      public ObservableCollection<CPitBoxSet> PitcherBoxVis { get; set; }
      //public ObservableCollection<CPitBoxSet> PitcherBoxHome { get; set; }

      public CBatBoxSet BatterBoxVis_tot { get; set; }
      private CGame mGame;
      public string VisName { get { return mGame?.t[0].nick ?? "Visitor"; } }
      public string HomeName { get { return mGame?.t[1].nick ?? "Home"; } }

      public BoxScoreListViewModel()
      {
      // -------------------------------------------------------------
      // This version of the constructor just build a dummy with
      // 9 batters and 1 pitcher and all 0's, for initial data binding
      // before a game starts.
      // -------------------------------------------------------------

         BatterBoxVis = new ObservableCollection<CBatBoxSet>();
         for (int i = 1; i <= 9; i++) {
            CBatBoxSet bs = new() { boxName = $"Batter {i}", bx = i };
            BatterBoxVis.Add(bs);
         }
         CBatBoxSet bsTot = new() { boxName = "Total" };
         BatterBoxVis.Add(bsTot); //This is the totals row.

         PitcherBoxVis = new ObservableCollection<CPitBoxSet>();
         CPitBoxSet ps = new() { boxName = "Pitcher 1", px = 1 };
         PitcherBoxVis.Add(ps);

         BatterBoxVis_tot = bsTot;

      }

        public BoxScoreListViewModel(CGame g, int side = 0) {
      // ------------------------------------------------------
         mGame = g;
         CBatter bat;
         CPitcher pit;
         int bx, px;

         BatterBoxVis = new ObservableCollection<CBatBoxSet>();
         var bsTot = new CBatBoxSet() { boxName = "Total"};
         for (int i = 1; i <= CGame.SZ_BAT; i++) {
            bx = g.t[side].xbox[i];
            if (bx == 0) break;
            bat = g.t[side].bat[bx];
            BatterBoxVis.Add(bat.bs);
            bsTot += bat.bs; //Operator overload! 
         }
         BatterBoxVis.Add(bsTot); //This is the totals row.


         //BatterBoxHome = new ObservableCollection<CBatBoxSet>();
         //for (int i = 1; i <= CGame.SZ_BAT; i++) {
         //   bx = g.t[1].xbox[i];
         //   if (bx == 0) break;
         //   bat = g.t[1].bat[bx];
         //   BatterBoxHome.Add(bat.bs);
         //}


         PitcherBoxVis = new ObservableCollection<CPitBoxSet>();
         for (int i = 1; i <= CGame.SZ_PIT; i++) {
            px = g.t[side].ybox[i];
            if (px == 0) break;
            pit = g.t[side].pit[px];
            pit.ps.boxName = pit.pname2;
            /*for (int j=1; j<=10; j++)*/ PitcherBoxVis.Add(pit.ps);
         }


         //PitcherBoxHome = new ObservableCollection<CPitBoxSet>();
         //for (int i = 1; i <= CGame.SZ_PIT; i++) {
         //   px = g.t[1].xbox[i];
         //   if (px == 0) break;
         //   pit = g.t[1].pit[px];
         //   PitcherBoxHome.Add(pit.ps);
         //}

         //var bs1 = new CBatBoxSet();
         foreach (var bs in BatterBoxVis) bsTot = bsTot + bs; //Operator overload!
         BatterBoxVis_tot = bsTot;

      }

      public event PropertyChangedEventHandler PropertyChanged;
      void OnPropertyChanged([CallerMemberName] string propertyName = "") {

         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

   }
}
