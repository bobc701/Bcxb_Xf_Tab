using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BCX.BCXB;

namespace BcxbXf.Models {

   public class BoxScoreListViewModel : INotifyPropertyChanged { 

      private CGame _mGame;
      private string _VisName = "Visitor", _HomeName = "Home";

      private CBatBoxSet _bsTot = new() { boxName = "Total" };
      private ObservableCollection<CBatBoxSet> batterBox;
      private ObservableCollection<CPitBoxSet> pitcherBox;


      public string VisName { get { return _VisName; } set { _VisName = value; OnPropertyChanged(); } }
      public string HomeName { get { return _HomeName; } set { _HomeName = value; OnPropertyChanged(); } }
      public ObservableCollection<CBatBoxSet> BatterBoxVis { get; set; } = new();
      public ObservableCollection<CBatBoxSet> BatterBoxHome { get; set; } = new();
      public ObservableCollection<CPitBoxSet> PitcherBoxVis { get; set; } = new();
      public ObservableCollection<CPitBoxSet> PitcherBoxHome { get; set; } = new();


      public BoxScoreListViewModel()
      {
         Debug.WriteLine("In BoxScoreListViewModel ctor");
      }

      public void Rebuild(CGame g, int side = 0) 
      {
         Debug.WriteLine($"In Rebuild side={side}");
         CBatter bat;
         CPitcher pit;
         int bx, px;
         batterBox = side switch { 0 => BatterBoxVis, 1 => BatterBoxHome, _ => null };
         pitcherBox = side switch { 0 => PitcherBoxVis, 1 => PitcherBoxHome, _ => null };

         if (side == 0) VisName = g.t[side].nick;
         else HomeName = g.t[side].nick;

         // Batter box...
         Debug.WriteLine($"In Rebuild: Starting batters...");
         _bsTot.Zero();
         batterBox.Clear();
         for (int i = 1; i <= CGame.SZ_BAT-1; i++) {
            Debug.WriteLine($"In Rebuild({side}), Batter {i}");
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
            Debug.WriteLine($"In Rebuild({side}), Pitcher {i}");
            px = g.t[side].ybox[i];
            if (px == 0) break;
            pit = g.t[side].pit[px];
            pit.ps.boxName = pit.pname2;
            pitcherBox.Add(pit.ps);
         }

         Debug.WriteLine($"Leaving Rebuild side={side}, Counts: {batterBox.Count()} {pitcherBox.Count()}");
      }


      public event PropertyChangedEventHandler PropertyChanged;
      void OnPropertyChanged([CallerMemberName] string propertyName = "") {

         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

   }
}
