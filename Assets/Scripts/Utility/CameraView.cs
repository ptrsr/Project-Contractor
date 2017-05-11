using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraView
{
    public static bool CheckInView(Vector3 position)
    {
        Vector3 view = Camera.main.WorldToViewportPoint(position);
        
        if (view.x < -0.05f || view.x > 1.05f || view.y < -0.2f || view.y > 1.025f)
            return false;
        else
            return true;
    }

    public static float ScreenRotation(Vector3 position)
    {
        Vector3 view = Camera.main.WorldToViewportPoint(position);

        if (view.x < 0f)
            return 180f;
        else if (view.x > 1f)
            return 0f;
        else if (view.y < 0f)
            return -90f;
        else if (view.y > 1f)
            return 90f;
        else
            return 0f;
    }

    private static Vector3 GetViewportPoint(Vector3 target)
    {
        Camera mainCam = Camera.main;
        Vector3 view = mainCam.WorldToViewportPoint(target);

        float x = 0f;
        float y = 0f;
        
        x = view.x < 0.04f ? 0.04f : (view.x > 0.96f ? 0.96f : view.x);
        y = view.y < 0.04f ? 0.1f : (view.y > 0.85f ? 0.91f : view.y + 0.06f);

        return new Vector3(x, y, view.z);
    }

    public static Vector3 GetRawBorderPosition(Vector3 target)
    {
        Camera mainCam = Camera.main;
        Vector3 view = mainCam.WorldToViewportPoint(target);

        float x = 0f;
        float y = 0f;

        x = view.x < 0f ? 0f : (view.x > 1f ? 1f : view.x);
        y = view.y < 0f ? 0f : (view.y > 1f ? 1f : view.y);

        return mainCam.ViewportToWorldPoint(new Vector3(x, y, view.z));
    }

    public static Vector3 GetBorderPosition(Vector3 target)
    {
        return Camera.main.ViewportToWorldPoint(GetViewportPoint(target));
    }

    public static Vector3 GetBorderPositionHUD(Vector3 target)
    {
        return Camera.main.ViewportToScreenPoint(GetViewportPoint(target));
    }
}