using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jellyScript : MonoBehaviour
{
    private CubeMovement _cubeMovement;
    private bool movementComplated;
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;
    public float rotationIntensity = 1f; // Rotasyonun yoğunluğu
    public float rotationResetSpeed = 2f; // Y ekseninin sıfıra dönme hızı

    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;

    private float totalForceMagnitude = 0f; // Toplam kuvvet büyüklüğü
    private float currentYRotation = 0f; // Mevcut Y ekseni rotasyonu
    private Vector3 previousPosition; // Objede önceki pozisyon

    void Start()
    {
        _cubeMovement = GetComponentInParent<CubeMovement>();
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));

        previousPosition = transform.position; // Başlangıç pozisyonunu kaydet
        _cubeMovement.StopIT += Stop;
    }

    private void OnDestroy()
    {
        _cubeMovement.StopIT -= Stop;
    }

    void Stop()
    {
        movementComplated = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void FixedUpdate()
    {
        
       
        
        vertexArray = OriginalMesh.vertices;
        totalForceMagnitude = 0f; // Her karede sıfırlanır

        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;

            // Her vertex'in hareketini güncelle
            jv[i].Shake(target, Mass, stiffness, damping);

            // Kuvvet büyüklüğünü hesapla
            totalForceMagnitude += jv[i].Force.magnitude;

            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }

        MeshClone.vertices = vertexArray;

        // Toplam kuvvete dayalı olarak Y ekseninde rotasyon uygula
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        // Objeyi hareket ettirme yönünü hesapla
        Vector3 movementDirection = transform.position - previousPosition;
        float horizontalMovement = movementDirection.x; // X yönündeki hareket

        if (totalForceMagnitude > 0.01f && Mathf.Abs(horizontalMovement) > 0.001f)
        {
            // Hareket varsa Y ekseni rotasyonunu hesapla
            float rotationAmount = horizontalMovement * rotationIntensity; // Hareket yönüne göre pozitif veya negatif
            currentYRotation = Mathf.Lerp(currentYRotation, rotationAmount, Time.fixedDeltaTime * 10f);
        }
        else
        {
            // Hareket yoksa Y eksenini sıfıra döndür
            currentYRotation = Mathf.Lerp(currentYRotation, 0f, Time.fixedDeltaTime * rotationResetSpeed);
        }

        if (!movementComplated)
        {
            transform.rotation = Quaternion.Euler(8, currentYRotation*60f, transform.rotation.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
       

        // Mevcut pozisyonu kaydet
        previousPosition = transform.position;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < 0.001f)
            {
                Position = target;
            }
        }
    }
}
