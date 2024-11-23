using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ExplodeCube : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private float _spawnRange = 3f;
    [SerializeField] private float _splitChance = 1f;
    [SerializeField] private float _explosionForce = 5000f;
    [SerializeField] private float _explosionRadius = 5f;

    private int _minCubes = 2;
    private int _maxCubes = 6;

    private void OnMouseDown()
    {
        float chanceToDestroy = Random.Range(0f, 1f);
        int cubesCount = Random.Range(_minCubes, _maxCubes + 1);
        float cubeVolume;

        List<GameObject> cubes = new List<GameObject>();

        Vector3 spawnPosition;
        Vector3 curentScale = transform.localScale;
        Rigidbody createdCubeRigidbody;

        if (chanceToDestroy > _splitChance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _splitChance *= 0.5f;

            Destroy(gameObject);

            for (int i = 0; i < cubesCount; i++)
            {
                spawnPosition = new Vector3(
                    transform.position.x + Random.Range(-_spawnRange, _spawnRange),
                    transform.position.y,
                    transform.position.z + Random.Range(-_spawnRange, _spawnRange));

                GameObject newCube = Instantiate(_cube, spawnPosition, Quaternion.identity);

                newCube.transform.localScale = curentScale * 0.5f;

                if (newCube.transform.localScale.magnitude <= 0.1f)
                {
                    Destroy(newCube);
                    continue;
                }

                createdCubeRigidbody = newCube.GetComponent<Rigidbody>();
                cubeVolume = newCube.transform.localScale.x * newCube.transform.localScale.y * newCube.transform.localScale.z;
                createdCubeRigidbody.mass = cubeVolume;

                Renderer cubeRenderer = newCube.GetComponent<Renderer>();

                if (cubeRenderer != null)
                {
                    cubeRenderer.material.color = new Color(
                        Random.value,
                        Random.value,
                        Random.value);
                }

                cubes.Add(newCube);
            }

            Rigidbody cubeRigidbody;

            foreach (GameObject cube in cubes)
            {
                cubeRigidbody = cube.GetComponent<Rigidbody>();

                cubeRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}
