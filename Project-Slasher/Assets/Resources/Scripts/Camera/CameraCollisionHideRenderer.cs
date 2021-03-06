using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraCollisionHideRenderer : MonoBehaviour
{
    public LayerMask hideMask;
    public Material passthroughMat;
    public CinemachineVirtualCamera vcam;

    internal class meshPair
    {
        public meshPair(int count, Material mat)
        {
            this.mat = mat;
            this.count = count;
        }
        public int count = 0;
        public float time = 0f;
        public Material mat;
    };

    private Dictionary<Renderer, meshPair> cache = new Dictionary<Renderer, meshPair>();

    private void OnTriggerEnter(Collider other)
    {
        var group = other.GetComponent<ColliderMerger>();
        if(group != null && group.gameObject.IsInLayerMask(hideMask))
        {
            foreach (var ren in group.AttachedRenderers)
                AddMeshRen(ren);
            return;
        }
        var meshRen = other.GetComponent<Renderer>();
        if (meshRen != null && meshRen.gameObject.IsInLayerMask(hideMask))
        {
            AddMeshRen(meshRen);
        }
    }

    private void AddMeshRen(Renderer meshRen)
    {
        if (!cache.ContainsKey(meshRen))
        {
            cache.Add(meshRen, new meshPair(1, meshRen.material));
            meshRen.material = passthroughMat;
            // Disable shadows to avoid weird dotted shadows
            meshRen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        else
            cache[meshRen].count++;
    }

    private void Update()
    {
        foreach(var pair in cache)
        {
            pair.Value.time += Time.deltaTime;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            var ren = pair.Key;
            ren.GetPropertyBlock(block);
            float t = Mathf.SmoothStep(0f, 1f, pair.Value.time / 2.5f);
            block.SetFloat("_Alpha", Mathf.Lerp(0.25f, 0.6f, t));
            ren.SetPropertyBlock(block);
        }
        transform.LookAt(vcam.m_Follow.transform.position + Vector3.up * 1.6f);
    }

    private void OnTriggerExit(Collider other)
    {
        var group = other.GetComponent<ColliderMerger>();
        if (group != null && group.gameObject.IsInLayerMask(hideMask))
        {
            foreach (var ren in group.AttachedRenderers)
                ClearMeshRen(ren);
            return;
        }
        var meshRen = other.GetComponent<Renderer>();
        if (meshRen != null && meshRen.gameObject.IsInLayerMask(hideMask))
        {
            ClearMeshRen(meshRen);
        }
    }

    private void ClearMeshRen(Renderer meshRen)
    {
        if (cache.ContainsKey(meshRen))
        {
            cache[meshRen].count--;
            if (cache[meshRen].count == 0)
            {
                meshRen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                meshRen.material = cache[meshRen].mat;
                cache.Remove(meshRen);
            }
        }
    }
}
