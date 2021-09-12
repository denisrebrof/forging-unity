using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public static class TextureMath
{
    private static int downSampleRes = 64;

    private static List<AsyncGPUReadbackRequest> requests = new List<AsyncGPUReadbackRequest>();

    public static void RemoveAsyncRequests() => requests.Clear();

    public static void CalcPercentColorRtAsync(Texture source, Action<float> onCalc)
    {
        RenderTexture downSampled = RenderTexture.GetTemporary(downSampleRes, downSampleRes, 32);
        Graphics.Blit(source, downSampled);

        requests.Add(AsyncGPUReadback.Request(downSampled, 0, req => OnRequestTextureSuccess(req, downSampleRes*downSampleRes, onCalc, downSampled)));
    }

    private static void OnRequestTextureSuccess(AsyncGPUReadbackRequest req, float fullpaintPixels, Action<float> onCalc, RenderTexture downsampled)
    {
        if(!requests.Contains(req))
            return;

        if(req.hasError)
        {
            requests.Remove(req);
            return;
        }

        int paintCount = 0;
        NativeArray<Color32> pixels = new NativeArray<Color32>(req.GetData<Color32>(), Allocator.TempJob);

        for(int i = 0;i < pixels.Length;i++)
        {
            if(pixels[i].a / 255f > 0.99f)
                paintCount++;
        }

        RenderTexture.ReleaseTemporary(downsampled);

        onCalc?.Invoke(paintCount / fullpaintPixels);

        pixels.Dispose();

        requests.Remove(req);
    }
}
