﻿using GTA5Core.Native;
using GTA5Core.Offsets;

namespace GTA5Core.Features;

public static class Teleport
{
    /// <summary>
    /// 传送到导航点
    /// </summary>
    public static void ToWaypoint()
    {
        SetTeleportPosition(WaypointPosition());
    }

    /// <summary>
    /// 传送到目标点
    /// </summary>
    public static void ToObjective()
    {
        SetTeleportPosition(ObjectivePosition());
    }

    /// <summary>
    /// 传送到Blips
    /// </summary>
    public static void ToBlips(int blipId)
    {
        SetTeleportPosition(GetBlipPosition(blipId));
    }

    /// <summary>
    /// 传送到Blips（带颜色ID）
    /// </summary>
    public static void ToBlips(int blipId, byte blipColor)
    {
        if (blipColor == 0)
            SetTeleportPosition(GetBlipPosition(blipId));
        else
            SetTeleportPosition(GetBlipPosition(blipId, blipColor));
    }

    /// <summary>
    /// 获取玩家当前坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPlayerPosition()
    {
        long pCPed = Game.GetCPed();
        return Memory.Read<Vector3>(pCPed + CPed.VisualX);
    }

    /// <summary>
    /// 坐标传送功能
    /// </summary>
    public static void SetTeleportPosition(Vector3 vector3)
    {
        if (vector3 != Vector3.Zero)
        {
            long pCPed = Game.GetCPed();

            if (Memory.Read<int>(pCPed + CPed.InVehicle) == 0)
            {
                // 玩家不在载具
                long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);
                Memory.Write(pCPed + CPed.VisualX, vector3);
                Memory.Write(pCNavigation + CNavigation.PositionX, vector3);
            }
            else
            {
                // 玩家在载具
                long pCVehicle = Memory.Read<long>(pCPed + CPed.CVehicle);
                Memory.Write(pCVehicle + CVehicle.VisualX, vector3);
                long pCNavigation = Memory.Read<long>(pCVehicle + CVehicle.CNavigation);
                Memory.Write(pCNavigation + CNavigation.PositionX, vector3);
            }
        }
    }

    /// <summary>
    /// 获取Blip坐标
    /// </summary>
    public static Vector3 GetBlipPosition(int blipIds)
    {
        for (int i = 1; i <= 2000; i++)
        {
            long pBlip = Memory.Read<long>(Pointers.BlipPTR + i * 0x08);
            if (!Memory.IsValid(pBlip))
                continue;

            var dwIcon = Memory.Read<int>(pBlip + 0x40);

            if (blipIds == dwIcon)
            {
                var vector3 = Memory.Read<Vector3>(pBlip + 0x10);
                vector3.Z += +1.0f;

                return vector3;
            }
        }

        return Vector3.Zero;
    }

    /// <summary>
    /// 获取Blip坐标
    /// </summary>
    public static Vector3 GetBlipPosition(int blipIds, byte blipColors)
    {
        for (int i = 1; i <= 2000; i++)
        {
            long pBlip = Memory.Read<long>(Pointers.BlipPTR + i * 0x08);
            if (!Memory.IsValid(pBlip))
                continue;

            var dwIcon = Memory.Read<int>(pBlip + 0x40);
            var dwColor = Memory.Read<byte>(pBlip + 0x48);

            //if (dwIcon== blipIds)
            //{
            //    Debug.WriteLine($"{dwIcon} {dwColor}");
            //}

            if (blipIds == dwIcon && blipColors == dwColor)
            {
                var vector3 = Memory.Read<Vector3>(pBlip + 0x10);
                vector3.Z += +1.0f;

                return vector3;
            }
        }

        return Vector3.Zero;
    }

    /// <summary>
    /// 获取Blip坐标
    /// </summary>
    public static Vector3 GetBlipPosition(int[] blipIds, byte[] blipColors, bool isUser = false)
    {
        for (int i = 1; i <= 2000; i++)
        {
            long pBlip = Memory.Read<long>(Pointers.BlipPTR + i * 0x08);
            if (!Memory.IsValid(pBlip))
                continue;

            var dwIcon = Memory.Read<int>(pBlip + 0x40);
            var dwColor = Memory.Read<byte>(pBlip + 0x48);

            if (blipIds.Contains(dwIcon) && blipColors.Contains(dwColor))
            {
                var vector3 = Memory.Read<Vector3>(pBlip + 0x10);

                if (isUser)
                    vector3.Z = vector3.Z == 20.0f ? -225.0f : vector3.Z + 1.0f;
                else
                    vector3.Z += +1.0f;

                return vector3;
            }
        }

        return Vector3.Zero;
    }

    /// <summary>
    /// 获取导航点坐标
    /// </summary>
    public static Vector3 WaypointPosition()
    {
        return GetBlipPosition(new int[] { 8 }, new byte[] { 84 }, true);
    }

    /// <summary>
    /// 获取目标点坐标
    /// </summary>
    public static Vector3 ObjectivePosition()
    {
        Vector3 vector3;

        vector3 = GetBlipPosition(new int[] { 1 }, new byte[] { 5, 60, 66 });
        if (vector3 != Vector3.Zero)
            return vector3;

        vector3 = GetBlipPosition(new int[] { 1, 225, 427, 478, 501, 523, 556 }, new byte[] { 1, 2, 3, 54, 78 });
        if (vector3 != Vector3.Zero)
            return vector3;

        vector3 = GetBlipPosition(new int[] { 432, 443 }, new byte[] { 59 });
        if (vector3 != Vector3.Zero)
            return vector3;

        return vector3;
    }

    /// <summary>
    /// 坐标向前微调
    /// </summary>
    public static void MoveFoward(float distance)
    {
        long pCPed = Game.GetCPed();
        long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        float head = Memory.Read<float>(pCNavigation + CNavigation.RightX);
        float head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        Vector3 vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X -= head2 * distance;
        vector3.Y += head * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向后微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveBack(float distance)
    {
        long pCPed = Game.GetCPed();
        long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        float head = Memory.Read<float>(pCNavigation + CNavigation.RightX);
        float head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        Vector3 vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X += head2 * distance;
        vector3.Y -= head * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向左微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveLeft(float distance)
    {
        long pCPed = Game.GetCPed();
        long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        float head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        Vector3 vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X += distance;
        vector3.Y -= head2 * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向右微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveRight(float distance)
    {
        long pCPed = Game.GetCPed();
        long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        float head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        Vector3 vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X -= distance;
        vector3.Y += head2 * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向上微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveUp(float distance)
    {
        Vector3 vector3 = GetPlayerPosition();
        vector3.Z += distance;
        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向下微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveDown(float distance)
    {
        Vector3 vector3 = GetPlayerPosition();
        vector3.Z -= distance;
        SetTeleportPosition(vector3);
    }

    //////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 传送到导航点（精准）
    /// </summary>
    public static async void ToWaypointSuper()
    {
        Vector3 coords = WaypointPosition();
        if (coords == Vector3.Zero)
            return;

        if (coords.Z == -225.0f)
        {
            bool isFindGround = false;
            float oldHeight = GetGroundZCoord();

            for (float z = 0; z < 1000; z += 100)
            {
                coords.Z = z;
                SetTeleportPosition(coords);

                coords.Z = GetGroundZCoord();
                if (coords.Z != 0.0f && coords.Z != oldHeight)
                {
                    isFindGround = true;
                    coords.Z += 1.0f;
                    break;
                }

                await Task.Delay(100);
            }

            if (!isFindGround)
                coords.Z = -301.0f;
        }

        SetTeleportPosition(coords);
    }

    /// <summary>
    /// 获取高度坐标
    /// </summary>
    public static float GetGroundZCoord()
    {
        if (Pointers.HeightPTR == 0x0)
        {
            Pointers.HeightPTR = Memory.FindPattern(Mask.Height);
            if (Pointers.HeightPTR == 0x0)
            {
                return -225.0f;
            }
        }
        return Memory.Read<float>(Pointers.HeightPTR + Base.GroundHeight);
    }
}
