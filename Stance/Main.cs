using GTA;
using GTA.Native;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stance
{
  public class Main : Script
  {
    private Main.Stance currentStance;
    private Stopwatch buttonPressTimer = new Stopwatch();

    public Main()
    {
      this.Requests();
      this.Tick += new EventHandler(this.OnTick);
      if (StanceSettings.overrideStealth)
        return;
      this.KeyDown += new KeyEventHandler(this.OnKeyDown);
      this.KeyUp += new KeyEventHandler(this.OnKeyUp);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != StanceSettings.stanceKey)
        return;
      this.buttonPressTimer.Start();
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
      if (this.currentStance == Main.Stance.Standing)
      {
        if (e.KeyCode != StanceSettings.stanceKey)
          return;
        if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
        {
          if (Game.Player.Character.IsSprinting || Game.Player.Character.IsRunning)
          {
            Game.Player.Character.Task.ClearAll();
            Game.Player.Character.Task.PlayAnimation("move_jump", "dive_start_run", 8f, -1, false, 0.0f);
            Script.Wait(1000);
            this.SetStance(Main.Stance.Prone);
          }
          else
            this.SetStance(Main.Stance.Prone);
        }
        else
          this.SetStance(Main.Stance.Crouching);
        this.buttonPressTimer.Stop();
        this.buttonPressTimer.Reset();
      }
      else if (this.currentStance == Main.Stance.Crouching)
      {
        if (e.KeyCode != StanceSettings.stanceKey)
          return;
        if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
          this.SetStance(Main.Stance.Prone);
        else
          this.SetStance(Main.Stance.Standing);
        this.buttonPressTimer.Stop();
        this.buttonPressTimer.Reset();
      }
      else
      {
        if (this.currentStance != Main.Stance.Prone || e.KeyCode != StanceSettings.stanceKey)
          return;
        if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
          this.SetStance(Main.Stance.Standing);
        else
          this.SetStance(Main.Stance.Crouching);
        this.buttonPressTimer.Stop();
        this.buttonPressTimer.Reset();
      }
    }

    private void OnTick(object sender, EventArgs e)
    {
      if (Game.Player.Character.IsInVehicle())
        return;
      if (StanceSettings.overrideStealth)
      {
        this.DisableControls();
        this.StanceChanger();
      }
      this.ProneMovement();
    }

    private void Requests()
    {
      Function.Call((Hash) 7972635428772450029L, new InputArgument[1]
      {
        InputArgument.op_Implicit("move_ped_crouched")
      });
      Function.Call((Hash) 7972635428772450029L, new InputArgument[1]
      {
        InputArgument.op_Implicit("move_ped_crouched_strafing")
      });
      Function.Call((Hash) -3189321952077349130L, new InputArgument[1]
      {
        InputArgument.op_Implicit("move_crawl")
      });
    }

    private void ProneMovement()
    {
      if (this.currentStance != Main.Stance.Prone)
        return;
      if (Game.IsControlPressed(0, (Control) 34))
      {
        Ped character = Game.Player.Character;
        ((Entity) character).Heading = ((Entity) character).Heading + 2f;
      }
      else if (Game.IsControlPressed(0, (Control) 35))
      {
        Ped character = Game.Player.Character;
        ((Entity) character).Heading = ((Entity) character).Heading - 2f;
      }
      if (Game.IsControlPressed(0, (Control) 32))
      {
        if (Function.Call<bool>((Hash) 2237014829242392265L, new InputArgument[4]
        {
          InputArgument.op_Implicit(Game.Player.Character),
          InputArgument.op_Implicit("move_crawl"),
          InputArgument.op_Implicit("onfront_fwd"),
          InputArgument.op_Implicit(3)
        }) == 0)
          Function.Call((Hash) -1565002832890405996L, new InputArgument[11]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit("move_crawl"),
            InputArgument.op_Implicit("onfront_fwd"),
            InputArgument.op_Implicit(8f),
            InputArgument.op_Implicit(-4f),
            InputArgument.op_Implicit(-1),
            InputArgument.op_Implicit(9),
            InputArgument.op_Implicit(0.0f),
            InputArgument.op_Implicit(false),
            InputArgument.op_Implicit(false),
            InputArgument.op_Implicit(false)
          });
      }
      else if (Game.IsControlPressed(0, (Control) 33))
      {
        if (Function.Call<bool>((Hash) 2237014829242392265L, new InputArgument[4]
        {
          InputArgument.op_Implicit(Game.Player.Character),
          InputArgument.op_Implicit("move_crawl"),
          InputArgument.op_Implicit("onfront_bwd"),
          InputArgument.op_Implicit(3)
        }) == 0)
          Function.Call((Hash) -1565002832890405996L, new InputArgument[11]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit("move_crawl"),
            InputArgument.op_Implicit("onfront_bwd"),
            InputArgument.op_Implicit(8f),
            InputArgument.op_Implicit(-4f),
            InputArgument.op_Implicit(-1),
            InputArgument.op_Implicit(9),
            InputArgument.op_Implicit(0.0f),
            InputArgument.op_Implicit(false),
            InputArgument.op_Implicit(false),
            InputArgument.op_Implicit(false)
          });
      }
      else if (Game.IsControlJustReleased(0, (Control) 32) || Game.IsControlJustReleased(0, (Control) 33))
      {
        Game.Player.Character.Task.ClearAll();
        Function.Call((Hash) 8798111594244947200L, new InputArgument[4]
        {
          InputArgument.op_Implicit(Game.Player.Character),
          InputArgument.op_Implicit((int) Function.Call<int>((Hash) -3292914402564945716L, new InputArgument[1]
          {
            InputArgument.op_Implicit("SCRIPTED_GUN_TASK_PLANE_WING")
          })),
          InputArgument.op_Implicit(1),
          InputArgument.op_Implicit(1)
        });
      }
    }

    private void StanceChanger()
    {
      if (this.currentStance == Main.Stance.Standing)
      {
        if (Game.IsControlPressed(0, (Control) 36))
        {
          this.buttonPressTimer.Start();
        }
        else
        {
          if (!Game.IsControlJustReleased(0, (Control) 36))
            return;
          if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
          {
            if (Game.Player.Character.IsSprinting || Game.Player.Character.IsRunning)
            {
              Game.Player.Character.Task.ClearAll();
              Game.Player.Character.Task.PlayAnimation("move_jump", "dive_start_run", 8f, -1, false, 0.0f);
              Script.Wait(1000);
              this.SetStance(Main.Stance.Prone);
            }
            else
              this.SetStance(Main.Stance.Prone);
          }
          else
            this.SetStance(Main.Stance.Crouching);
          this.buttonPressTimer.Stop();
          this.buttonPressTimer.Reset();
        }
      }
      else if (this.currentStance == Main.Stance.Crouching)
      {
        if (Game.IsControlPressed(0, (Control) 36))
        {
          this.buttonPressTimer.Start();
        }
        else
        {
          if (!Game.IsControlJustReleased(0, (Control) 36))
            return;
          if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
            this.SetStance(Main.Stance.Prone);
          else
            this.SetStance(Main.Stance.Standing);
          this.buttonPressTimer.Stop();
          this.buttonPressTimer.Reset();
        }
      }
      else
      {
        if (this.currentStance != Main.Stance.Prone)
          return;
        if (Game.IsControlPressed(0, (Control) 36))
          this.buttonPressTimer.Start();
        else if (Game.IsControlJustReleased(0, (Control) 36))
        {
          if (this.buttonPressTimer.ElapsedMilliseconds >= 250L)
            this.SetStance(Main.Stance.Standing);
          else
            this.SetStance(Main.Stance.Crouching);
          this.buttonPressTimer.Stop();
          this.buttonPressTimer.Reset();
        }
      }
    }

    private void SetStance(Main.Stance stance)
    {
      switch (stance)
      {
        case Main.Stance.Standing:
          Function.Call((Hash) -6164042450715612628L, new InputArgument[2]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit(1048576000)
          });
          Function.Call((Hash) 2328651364711703671L, new InputArgument[1]
          {
            InputArgument.op_Implicit(Game.Player.Character)
          });
          Game.Player.Character.Task.ClearAll();
          this.currentStance = Main.Stance.Standing;
          break;
        case Main.Stance.Crouching:
          Function.Call((Hash) -5797657820774978577L, new InputArgument[3]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit("move_ped_crouched"),
            InputArgument.op_Implicit(1048576000)
          });
          Function.Call((Hash) 3000117804892870740L, new InputArgument[2]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit("move_ped_crouched_strafing")
          });
          Game.Player.Character.Task.ClearAll();
          this.currentStance = Main.Stance.Crouching;
          break;
        case Main.Stance.Prone:
          if (!(bool) Function.Call<bool>((Hash) 5140692576702410007L, new InputArgument[2]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit(6)
          }))
            break;
          Game.Player.Character.Task.ClearAllImmediately();
          Function.Call((Hash) 8798111594244947200L, new InputArgument[4]
          {
            InputArgument.op_Implicit(Game.Player.Character),
            InputArgument.op_Implicit((int) Function.Call<int>((Hash) -3292914402564945716L, new InputArgument[1]
            {
              InputArgument.op_Implicit("SCRIPTED_GUN_TASK_PLANE_WING")
            })),
            InputArgument.op_Implicit(1),
            InputArgument.op_Implicit(1)
          });
          this.currentStance = Main.Stance.Prone;
          break;
      }
    }

    private void DisableControls() => Game.DisableControlThisFrame(0, (Control) 36);

    private enum Stance
    {
      Standing,
      Crouching,
      Prone,
    }
  }
}
