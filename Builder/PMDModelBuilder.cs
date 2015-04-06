﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMD.Adapter.PMD;
using UnityEngine;
using MMD.Component;

namespace MMD.Builder.PMD
{
    public class ModelBuilder
    {
        public SkinnedMeshRenderer Renderer { get; set; }

        public Mesh Mesh { get; set; }

        public Material[] Materials { get; set; }
        public Texture[] Textures { get; set; }

        public GameObject[] Bones { get; set; }

        public GameObject[] Rigidbodies { get; set; }

        public ModelBuilder(SkinnedMeshRenderer renderer)
        {
            Mesh = new UnityEngine.Mesh();
            Renderer = renderer;
            Renderer.sharedMesh = Mesh;
        }

        public void Read(MMD.Format.PMDFormat format, Shader shader, float scale)
        {
            // メッシュの参照
            var modelAdapter = new ModelAdapter();
            modelAdapter.Read(format, scale);
            Mesh = modelAdapter.Mesh;

            // ボーンの参照
            var boneAdapter = new BoneAdapter();
            boneAdapter.Read(format.Bones, scale);
            Renderer.bones = boneAdapter.BoneTransforms.ToArray();
            Bones = boneAdapter.GameObjects.ToArray();

            // ウェイトの参照
            Mesh.boneWeights = boneAdapter.Weights(format.Vertices);

            // マテリアルの参照
            var materialAdapter = new MaterialAdapter();
            materialAdapter.Read(shader, format);

            Materials = materialAdapter.Materials.ToArray();
            Textures = materialAdapter.Textures.ToArray();

            // 剛体の参照
        }
    }
}
