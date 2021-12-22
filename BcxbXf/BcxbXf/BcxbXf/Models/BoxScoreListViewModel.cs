using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BCX.BCXB;

namespace BcxbXf.Models {

   public class BoxScoreListViewModel : INotifyPropertyChanged { 

      private CGame mGame;

        public string TeamName { get; set; }
        public ObservableCollection<CBatBoxSet> BatterBox { get; set; } = new();
        public ObservableCollection<CPitBoxSet> PitcherBox { get; set; } = new();

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
         bs = new() { boxName = "Total" };
         BatterBox.Add(bs); //This is the totals row for vis team.

         CPitBoxSet ps;
         ps = new() { boxName = $"{teamName} pitcher 1", px = 1 };
         PitcherBox.Add(ps);

         TeamName = teamName;
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

         TeamName = mGame.t[0].nick;

      // Visitor batter box...
         bsTot = new CBatBoxSet() { boxName = "Total"};
         for (int i = 1; i <= CGame.SZ_BAT; i++) {
            bx = g.t[side].xbox[i];
            if (bx == 0) break;
            bat = g.t[side].bat[bx];
            BatterBox.Add(bat.bs);
            bsTot += bat.bs; //Operator overload! 
         }
         BatterBox.Add(bsTot); //This is the totals row.

      // Visitor pitcher box...
         for (int i = 1; i <= CGame.SZ_PIT; i++) {
            px = g.t[side].ybox[i];
            if (px == 0) break;
            pit = g.t[side].pit[px];
            pit.ps.boxName = pit.pname2;
            PitcherBox.Add(pit.ps);
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
