﻿using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    //public GameObject Timer;
    private GameObject _lookedAtObject = null;
    private GameObject _currentlyHoldingObject = null;
    public static GameObject CurrentlyHoldingObjectForBeakers;
    private Transform _parentOfTemporaryObject = null;
    public Material DefaultReturnItemMaterial;
    public Material OriginalItemMaterial;
    public Material DefaultItemMaterial;
    public Material GlovesMaterial;
    public Material DefaultBeakerMaterial;
    public Material HoverableObjectMaterial;
    private MeshRenderer _meshRenderer;
    private MeshRenderer[] _meshRendererGloves;
    public LayerMask CatchItemLayer;
    public LayerMask ReturnObjectLayer;
    public GameObject Point;
    public GameObject PlayerBody;

    private Transform _tempPosition = null;
    private static bool _canReturnObject = false;
    
    public static bool IsWearingCoat = false;
    public static bool IsWearingGlasses = false;
    public static bool IsWearingGloves = false;
    private GameObject _coat;
    private Transform _coatReturnSpot;
    private GameObject _glasses;
    private Transform _glassesReturnSpot;
    private GameObject _gloves;
    private Transform _glovesReturnSpot;

    public GameObject LeftController;
    public GameObject RightController;

    public static bool IsHoldingItem = false;
    private bool _isMaterialTaken = false;
    private bool _isColliderSizeUpdated = false;

    private AudioSource _centerEyeAudioSource;

    private void Start()
    {
        _centerEyeAudioSource = GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>();
        _coat = GameObject.Find("Lab Coat");
        _glasses = GameObject.Find("Lab Glasses");
        _gloves = GameObject.Find("Lab Gloves");
        
        _coatReturnSpot = GameObject.Find("Lab Coat Return Spot").transform;
        _glassesReturnSpot = GameObject.Find("Lab Glasses Return Spot").transform;
        _glovesReturnSpot = GameObject.Find("Lab Gloves Return Spot").transform;
    }

    // Update is called once per frame
    void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        Ray pointRay = new Ray(transform.position, transform.forward);
        RaycastHit pointHit;

        if (Physics.Raycast(pointRay, out pointHit, 100))
        {
            Point.SetActive(true);
            // We want to visualize the point of impact where the
            // player is pointing it, so he knows where he is clicking.
            var pointPosition = Point.transform.position;
            pointPosition = pointHit.point;
            Point.transform.position = pointPosition;
        }
        else
        {
            // When nothing is being pointed at, disable the point of the pointer.

            Point.SetActive(false);
        }

        /*
        if (IsHoldingItem && _isColliderSizeUpdated == false)
        {
            Vector3 sizeOfCollider = GameObject.Find("Empty Beaker(Clone)").GetComponent<BoxCollider>().size;
            sizeOfCollider.y *= 10;
            GameObject.Find("Empty Beaker(Clone)").GetComponent<BoxCollider>().size = sizeOfCollider;
            _isColliderSizeUpdated = true;
        }
        */

        if (IsHoldingItem)
        {
            if (Physics.Raycast(ray, out hit, 100)
                )
            {
                if (hit.transform.tag == "Return Item" && (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                    Input.GetKeyDown(KeyCode.K)))
                {
                    if (_lookedAtObject != null)
                    {
                        _meshRenderer.material = OriginalItemMaterial;
                        _lookedAtObject = null;
                        _parentOfTemporaryObject = null;
                    }

                    _lookedAtObject = hit.collider.gameObject;
                    _parentOfTemporaryObject = _lookedAtObject.transform.parent;

                    _lookedAtObject.tag = "Untagged";
                    _lookedAtObject.GetComponent<MeshRenderer>().material = OriginalItemMaterial;
                    //_lookedAtObject.layer = 18;
                    if (_lookedAtObject.name != "Yellow Substance Beaker" &&
                        _lookedAtObject.name != "Red Substance Beaker" &&
                        _lookedAtObject.name != "Blue Substance Beaker" &&
                        _lookedAtObject.name != "Orange Substance Beaker" &&
                        _lookedAtObject.name != "Purple Substance Beaker" &&
                        _lookedAtObject.name != "Green Substance Beaker" &&
                        _lookedAtObject.name != "Water Beaker" &&
                        _lookedAtObject.name != "Empty Beaker" &&
                        _lookedAtObject.name != "Round Empty Beaker" &&
                        _lookedAtObject.name != "Big Empty Beaker" &&
                        _lookedAtObject.name != "Small Empty Beaker" &&
                        _lookedAtObject.name != "Dropper" &&
                        _lookedAtObject.name != "Empty Dropper")
                    {
                        //_meshRenderer.material = OriginalItemMaterial;
                    }
                    else
                    {
                        if (_currentlyHoldingObject.name == "Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }
                            _lookedAtObject.name = "Empty Beaker";
                        }
                        else if (_currentlyHoldingObject.name == "Big Empty Beaker(Clone)" ||
                          _currentlyHoldingObject.name == "Big Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                                _lookedAtObject.name = "Big Empty Beaker";
                            }
                        }
                        else if (_currentlyHoldingObject.name == "Round Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Round Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                                _lookedAtObject.name = "Round Empty Beaker";
                            }
                        }
                        else if (_currentlyHoldingObject.name == "Small Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Small Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }
                            _lookedAtObject.name = "Small Empty Beaker";
                        }
                        else if (_currentlyHoldingObject.transform.GetChild(0).name == "Smelly Waste(Clone)")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }

                            Instantiate(_currentlyHoldingObject.transform.GetChild(0).gameObject,
                                _lookedAtObject.transform.position,
                                Quaternion.identity,
                                _lookedAtObject.transform);
                        }

                        // If we are returning a dropper and it contains liquids inside it, then
                        // we return the dropper AND transfer the liquids to the original dropper.
                        if (((_currentlyHoldingObject.name == "Empty Dropper" ||
                            _currentlyHoldingObject.name == "Big Empty Beaker" ||
                            _currentlyHoldingObject.name == "Round Empty Beaker" ||
                            _currentlyHoldingObject.name == "Small Empty Beaker") ||
                            _currentlyHoldingObject.name == "Dropper(Clone)"))
                        {
                            // Destroy the existing liquid in the dropper because you dont want to
                            // keep placing liquid in the dropper, we only have one liquid substance in a dropper.
                            if (_currentlyHoldingObject.transform.childCount > 0)
                            {
                                Instantiate(_currentlyHoldingObject.transform.GetChild(0).gameObject,
                                _lookedAtObject.transform.position,
                                Quaternion.identity,
                                _lookedAtObject.transform);
                            }

                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }

                        }
                        else if ((_currentlyHoldingObject.name == "Empty Dropper" ||
                            _currentlyHoldingObject.name == "Empty Dropper(Clone)") &&
                            _lookedAtObject.transform.childCount > 0)
                        {
                            // If we are returning an empty dropper, then we have to
                            // destroy the existing liquids in the original dropper.
                            Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            _lookedAtObject.name = "Dropper";
                        }

                        //_meshRenderer.material = OriginalItemMaterial;
                        _currentlyHoldingObject.tag = "Untagged";
                    }

                    if (_lookedAtObject.name == "Water") { ObjectivesSelector.PlacedBackWaterBottle = true; }
                    Destroy(_currentlyHoldingObject);

                    // This gives a small delay before the player can return
                    // the object back to its place, because for some reason oculus
                    // had a glitch and returned it immediately and it was very buggy.
                    Invoke("EnableReturningObject", 0.1f);
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, 100, CatchItemLayer))
            {
                if (_lookedAtObject != null)
                {
                    if (_meshRenderer != null)
                    {
                        _meshRenderer.material = OriginalItemMaterial;
                    }
                    _lookedAtObject = null;
                    _parentOfTemporaryObject = null;
                }

                _lookedAtObject = hit.collider.gameObject;
                _parentOfTemporaryObject = _lookedAtObject.transform.parent;
                if (_lookedAtObject.GetComponent<MeshRenderer>() == null)
                {
                    _meshRenderer = _lookedAtObject.GetComponentInChildren<MeshRenderer>();
                }
                else
                {
                    _meshRenderer = _lookedAtObject.GetComponent<MeshRenderer>();
                }
                OriginalItemMaterial = _meshRenderer.material;

                _meshRenderer.material = HoverableObjectMaterial;

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                    Input.GetKeyDown(KeyCode.K))
                {
                    if (_lookedAtObject.name == "Water") { ObjectivesSelector.PickedUpWaterBottle = true; }

                    if (_lookedAtObject.name == "Lab Coat")
                    {
                        _centerEyeAudioSource.PlayOneShot(_centerEyeAudioSource.gameObject.GetComponent<CenterEyePointer>().LabCoatPutOnSound);

                        _coat = _lookedAtObject;
                        _coat.GetComponent<MeshRenderer>().material = DefaultItemMaterial;
                        _coat.GetComponent<BoxCollider>().isTrigger = true;
                        _coat.GetComponent<Rigidbody>().isKinematic = true;
                        _coat.GetComponent<MeshRenderer>().enabled = false;
                        IsWearingCoat = true;
                    }
                    else if (_lookedAtObject.name == "Lab Glasses")
                    {
                        _glasses = _lookedAtObject;
                        _glasses.GetComponentInChildren<MeshRenderer>().material = DefaultItemMaterial;
                        IsWearingGlasses = true;
                    }
                    else if (_lookedAtObject.name == "Lab Gloves")
                    {
                        _gloves = _lookedAtObject;
                        MeshRenderer[] glovesRenderers = _gloves.GetComponentsInChildren<MeshRenderer>();

                        IsWearingGloves = true;
                    }
                    else
                    {
                        _meshRenderer.material = OriginalItemMaterial;

                        _currentlyHoldingObject = Instantiate(_lookedAtObject, _parentOfTemporaryObject.transform);

                        if (_lookedAtObject.name != "Yellow Substance Beaker" &&
                            _lookedAtObject.name != "Red Substance Beaker" &&
                            _lookedAtObject.name != "Blue Substance Beaker" &&
                            _lookedAtObject.name != "Water Beaker" &&
                            _lookedAtObject.name != "Orange Substance Beaker" &&
                            _lookedAtObject.name != "Purple Substance Beaker" &&
                            _lookedAtObject.name != "Green Substance Beaker" &&
                            _lookedAtObject.name != "Empty Beaker" &&
                            _lookedAtObject.name != "Big Empty Beaker" &&
                            _lookedAtObject.name != "Round Empty Beaker" &&
                            _lookedAtObject.name != "Small Empty Beaker" &&
                            _lookedAtObject.name != "Dropper")
                        {
                            _currentlyHoldingObject.GetComponent<MeshRenderer>().material = OriginalItemMaterial;

                            if (_currentlyHoldingObject.GetComponent<FireExtinguisher>() != null)
                            {
                                _currentlyHoldingObject.GetComponent<FireExtinguisher>().IsHoldingExtinguisher = true;
                            }
                        }
                        else
                        {
                            _currentlyHoldingObject.GetComponent<MeshRenderer>().material = DefaultBeakerMaterial;
                            if (_lookedAtObject.name == "Dropper")
                            {
                                _currentlyHoldingObject.tag = "Dropper";
                            }
                            else
                            {
                                _currentlyHoldingObject.tag = "Beaker";
                            }
                        }

                        //_currentlyHoldingObject.layer = 22;
                        //_lookedAtObject.layer = 21;
                        CurrentlyHoldingObjectForBeakers = _currentlyHoldingObject;
                        _lookedAtObject.tag = "Return Item";
                        Invoke("EnableReturningObject", 0.1f);
                    }
                }
            }
            else
            {
                if (_meshRenderer != null)
                {
                    _meshRenderer.material = OriginalItemMaterial;
                }
                _lookedAtObject = null;
                _parentOfTemporaryObject = null;
            }
        }

        /*
        if (IsHoldingItem)
        {
            // If the player is holding an item and needs to return it, he must
            // be hovering over the original indicator for that item and press the
            // controller button to return the item and remove it from his hand.
            if (Physics.Raycast(ray, out hit, 100, ReturnObjectLayer)
                )
            {
                //_meshRenderer = _lookedAtObject.GetComponent<MeshRenderer>();
                //OriginalItemMaterial = _meshRenderer.material;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                    Input.GetKeyDown(KeyCode.K))
                {
                    _lookedAtObject.tag = "Untagged";
                    _lookedAtObject.layer = 18;
                    if (_lookedAtObject.name != "Yellow Substance Beaker" &&
                        _lookedAtObject.name != "Red Substance Beaker" &&
                        _lookedAtObject.name != "Blue Substance Beaker" &&
                        _lookedAtObject.name != "Orange Substance Beaker" &&
                        _lookedAtObject.name != "Purple Substance Beaker" &&
                        _lookedAtObject.name != "Green Substance Beaker" &&
                        _lookedAtObject.name != "Water Beaker" &&
                        _lookedAtObject.name != "Empty Beaker" &&
                        _lookedAtObject.name != "Round Empty Beaker" &&
                        _lookedAtObject.name != "Big Empty Beaker" &&
                        _lookedAtObject.name != "Small Empty Beaker" &&
                        _lookedAtObject.name != "Dropper" &&
                        _lookedAtObject.name != "Empty Dropper")
                    {
                        //_meshRenderer.material = OriginalItemMaterial;
                    }
                    else
                    {
                        if (_currentlyHoldingObject.name == "Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }
                            _lookedAtObject.name = "Empty Beaker";
                        } else if (_currentlyHoldingObject.name == "Big Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Big Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                                _lookedAtObject.name = "Big Empty Beaker";
                            }
                        }
                        else if (_currentlyHoldingObject.name == "Round Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Round Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                                _lookedAtObject.name = "Round Empty Beaker";
                            }
                        }
                        else if (_currentlyHoldingObject.name == "Small Empty Beaker(Clone)" ||
                            _currentlyHoldingObject.name == "Small Empty Beaker")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }
                            _lookedAtObject.name = "Small Empty Beaker";
                        } else if (_currentlyHoldingObject.transform.GetChild(0).name == "Smelly Waste(Clone)")
                        {
                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }

                            Instantiate(_currentlyHoldingObject.transform.GetChild(0).gameObject,
                                _lookedAtObject.transform.position,
                                Quaternion.identity,
                                _lookedAtObject.transform);
                        }

                        // If we are returning a dropper and it contains liquids inside it, then
                        // we return the dropper AND transfer the liquids to the original dropper.
                        if (((_currentlyHoldingObject.name == "Empty Dropper" ||
                            _currentlyHoldingObject.name == "Big Empty Beaker" ||
                            _currentlyHoldingObject.name == "Round Empty Beaker" ||
                            _currentlyHoldingObject.name == "Small Empty Beaker") ||
                            _currentlyHoldingObject.name == "Dropper(Clone)"))
                        {
                            // Destroy the existing liquid in the dropper because you dont want to
                            // keep placing liquid in the dropper, we only have one liquid substance in a dropper.
                            if (_currentlyHoldingObject.transform.childCount > 0)
                            {
                                Instantiate(_currentlyHoldingObject.transform.GetChild(0).gameObject,
                                _lookedAtObject.transform.position,
                                Quaternion.identity,
                                _lookedAtObject.transform);
                            }

                            if (_lookedAtObject.transform.childCount > 0)
                            {
                                Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            }

                        }
                        else if ((_currentlyHoldingObject.name == "Empty Dropper" ||
                            _currentlyHoldingObject.name == "Empty Dropper(Clone)") &&
                            _lookedAtObject.transform.childCount > 0)
                        {
                            // If we are returning an empty dropper, then we have to
                            // destroy the existing liquids in the original dropper.
                            Destroy(_lookedAtObject.transform.GetChild(0).gameObject);
                            _lookedAtObject.name = "Dropper";
                        }
                        
                        //_meshRenderer.material = OriginalItemMaterial;
                        _currentlyHoldingObject.tag = "Untagged";
                    }

                    if (_lookedAtObject.name == "Water") { ObjectivesSelector.PlacedBackWaterBottle = true; }
                    Destroy(_currentlyHoldingObject);
                    
                    // This gives a small delay before the player can return
                    // the object back to its place, because for some reason oculus
                    // had a glitch and returned it immediately and it was very buggy.
                    Invoke("EnableReturningObject", 0.1f);
                }
            }
        }
        else
        {
            // We are looking with the controller for an object
            // that is of layer item
            if (Physics.Raycast(ray, out hit, 100, CatchItemLayer))
            {
                _lookedAtObject = hit.collider.gameObject;
                _parentOfTemporaryObject = _lookedAtObject.transform.parent;

                if (_lookedAtObject.name != "Lab Gloves")
                {
                    if (_meshRenderer != null)
                    {
                        _meshRenderer.material = OriginalItemMaterial;
                        _isMaterialTaken = false;
                    }
                    if (_lookedAtObject.GetComponent<MeshRenderer>() == null)
                    {
                        _meshRenderer = _lookedAtObject.GetComponentInChildren<MeshRenderer>();
                    }
                    else
                    {
                        _meshRenderer = _lookedAtObject.GetComponent<MeshRenderer>();
                    }
                } else
                {
                    _gloves.GetComponent<MeshRenderer>().material = HoverableObjectMaterial;
                }
                // Once it is found we change it to look as if it is hovered
                // so that it appears interact-able
                if (_lookedAtObject.name != "Lab Gloves")
                {
                    if (_isMaterialTaken == false)
                    {
                        OriginalItemMaterial = _meshRenderer.material;
                        _isMaterialTaken = true;
                    }
                    if (_meshRenderer != null)
                    {
                        _meshRenderer.material = HoverableObjectMaterial;
                    }
                }
                
                // If the player is not holding an interactable item and presses
                // the controller button, we append the item onto his hand and
                // make the one he clicked on become transparent, to indicate where
                // it must be returned after it is done being used.
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
                    Input.GetKeyDown(KeyCode.K))
                {
                    if (_lookedAtObject.name == "Water") { ObjectivesSelector.PickedUpWaterBottle = true; }

                    if (_lookedAtObject.name == "Lab Coat")
                    {
                        _coat = _lookedAtObject;
                        _coat.GetComponent<MeshRenderer>().material = DefaultItemMaterial;
                        _coat.GetComponent<BoxCollider>().isTrigger = true;
                        _coat.GetComponent<Rigidbody>().isKinematic = true;
                        _coat.GetComponent<MeshRenderer>().enabled = false;
                        IsWearingCoat = true;
                    } else if (_lookedAtObject.name == "Lab Glasses")
                    {
                        _glasses = _lookedAtObject;
                        _glasses.GetComponentInChildren<MeshRenderer>().material = DefaultItemMaterial;
                        IsWearingGlasses = true;
                    }
                    else if (_lookedAtObject.name == "Lab Gloves")
                    {
                        _gloves = _lookedAtObject;
                        MeshRenderer[] glovesRenderers = _gloves.GetComponentsInChildren<MeshRenderer>();

                        foreach (MeshRenderer renderer in glovesRenderers)
                        {
                            //renderer.material = DefaultItemMaterial;
                        }
                        IsWearingGloves = true;
                    }
                    else
                    {
                        _currentlyHoldingObject = Instantiate(_lookedAtObject, _parentOfTemporaryObject.transform);
                        if (_lookedAtObject.name != "Yellow Substance Beaker" &&
                            _lookedAtObject.name != "Red Substance Beaker" &&
                            _lookedAtObject.name != "Blue Substance Beaker" &&
                            _lookedAtObject.name != "Water Beaker" &&
                            _lookedAtObject.name != "Orange Substance Beaker" &&
                            _lookedAtObject.name != "Purple Substance Beaker" &&
                            _lookedAtObject.name != "Green Substance Beaker" &&
                            _lookedAtObject.name != "Empty Beaker" &&
                            _lookedAtObject.name != "Big Empty Beaker" &&
                            _lookedAtObject.name != "Round Empty Beaker" &&
                            _lookedAtObject.name != "Small Empty Beaker" &&
                            _lookedAtObject.name != "Dropper")
                        {
                            _currentlyHoldingObject.GetComponent<MeshRenderer>().material = DefaultItemMaterial;
                            CurrentlyHoldingObjectForBeakers = _currentlyHoldingObject;
                        }
                        else
                        {
                            _currentlyHoldingObject.GetComponent<MeshRenderer>().material = DefaultBeakerMaterial;
                            if (_lookedAtObject.name == "Dropper")
                            {
                                _currentlyHoldingObject.tag = "Dropper";
                            }
                            else
                            {
                                _currentlyHoldingObject.tag = "Beaker";
                            }
                            CurrentlyHoldingObjectForBeakers = _currentlyHoldingObject;
                        }
                        _currentlyHoldingObject.layer = 22;
                
                        //_meshRenderer.material = DefaultReturnItemMaterial;
                        _lookedAtObject.layer = 21;
                        _lookedAtObject.tag = "Return Item";

                        Invoke("EnableReturningObject", 0.1f);   
                    }
                }
            }
            else
            {
                if (_meshRenderer != null)
                {
                    _meshRenderer.material = OriginalItemMaterial;
                }
                if (_gloves != null)
                {
                    _gloves.GetComponent<MeshRenderer>().material = GlovesMaterial;
                }
                /*
                if ((_meshRenderer != null || _meshRendererGloves != null) && IsHoldingItem == false)
                {
                    if (_lookedAtObject.name != "Yellow Substance Beaker" &&
                        _lookedAtObject.name != "Red Substance Beaker" &&
                        _lookedAtObject.name != "Blue Substance Beaker" &&
                        _lookedAtObject.name != "Water Beaker" &&
                        _lookedAtObject.name != "Orange Substance Beaker" &&
                        _lookedAtObject.name != "Purple Substance Beaker" &&
                        _lookedAtObject.name != "Green Substance Beaker" &&
                        _lookedAtObject.name != "Empty Beaker" &&
                        _lookedAtObject.name != "Big Empty Beaker" &&
                        _lookedAtObject.name != "Round Empty Beaker" &&
                        _lookedAtObject.name != "Small Empty Beaker" &&
                        _lookedAtObject.name != "Dropper")
                    {
                        if (_meshRendererGloves != null)
                        {
                            foreach (MeshRenderer renderer in _meshRendererGloves)
                            {
                                renderer.material = DefaultItemMaterial;
                            }
                        }
                    }
                    else
                    {
                        _meshRenderer.material = OriginalItemMaterial;
                        _isMaterialTaken = false;
                    }
                }
            }
        }
        
        */
        // If an item is to be returned, then it will be tracked and
        // highlighted in the world, because the rays were having trouble
        // updating its material.
        var itemToReturn = GameObject.FindGameObjectWithTag("Return Item");
        if (itemToReturn != null)
        {
            itemToReturn.GetComponent<MeshRenderer>().material = DefaultReturnItemMaterial;
        }

        // This makes sure the item the player is holding is being placed
        // at the spot where the controller hand is.
        if (ObjectivesSelector.CanWearEquipment)
        {
            if (IsHoldingItem && _currentlyHoldingObject != null)
            {
                var newItemPosition = _currentlyHoldingObject.transform.position;
                newItemPosition = transform.position;
                _currentlyHoldingObject.transform.position = newItemPosition;
				
                var newItemRotation = _currentlyHoldingObject.transform.rotation;
                newItemRotation = transform.rotation;
                _currentlyHoldingObject.transform.rotation = newItemRotation;
            }

            if (IsWearingCoat && _coat != null)
            {
                UpdatePosition(_coat, null);
            }

            if (IsWearingGlasses && _glasses != null)
            {
                UpdatePosition(_glasses, null);
            }

            if (IsWearingGloves && _gloves != null)
            {
                UpdatePosition(_gloves, null);
            }
        }
    }

    private void EnableReturningObject()
    {
        IsHoldingItem = !IsHoldingItem;
    }

    public void ReturnEquipment()
    {
        _coat.GetComponent<Rigidbody>().isKinematic = true;
        _coat.GetComponent<BoxCollider>().isTrigger = false;
        UpdatePosition(_coat, _coatReturnSpot);
        UpdatePosition(_glasses, _glassesReturnSpot);
        UpdatePosition(_gloves, _glovesReturnSpot);
    }

    private void UpdatePosition(GameObject equipment, Transform ReturnTransform)
    {
        // This places the desired equipment to its original location before
        // the player took it for wearing.
        if (ReturnTransform != null)
        {
            var newEquipmentPosition = equipment.transform.position;
            newEquipmentPosition = ReturnTransform.position;
            equipment.transform.position = newEquipmentPosition;

            if (equipment.transform.childCount > 0 && equipment.name != "Lab Glasses")
            {
                var rightGlovePosition = equipment.transform.GetChild(0).transform.position;
                rightGlovePosition = ReturnTransform.transform.position;
                equipment.transform.GetChild(0).transform.position = rightGlovePosition;

                /*
                var rightGloveRotation = equipment.transform.GetChild(0).transform.rotation;
                rightGloveRotation = ReturnTransform.transform.rotation;
                equipment.transform.GetChild(0).transform.rotation = rightGloveRotation;
                */
            }

            //var newEquipmentRotation = equipment.transform.rotation;
            //newEquipmentRotation = ReturnTransform.rotation;
            //equipment.transform.rotation = newEquipmentRotation;   
        }
        else
        {
            if (equipment.name == "Lab Gloves")
            {
                // Moving the RIGHT glove to the player's left controller hand
                var rightGlovePosition = equipment.transform.GetChild(0).transform.position;
                rightGlovePosition = RightController.transform.position;
                equipment.transform.GetChild(0).transform.position = rightGlovePosition;
				
                /*var rightGloveRotation = equipment.transform.GetChild(0).transform.rotation;
                rightGloveRotation = RightController.transform.rotation;
                equipment.transform.GetChild(0).transform.rotation = rightGloveRotation;
                */
            }
            else
            {
                var newEquipmentPosition = equipment.transform.position;
                newEquipmentPosition = PlayerBody.transform.position;
                if (equipment.name == "Lab Glasses")
                {
                    newEquipmentPosition.y += 1;
                }
                equipment.transform.position = newEquipmentPosition;
				
                var newEquipmentRotation = equipment.transform.rotation;
                newEquipmentRotation = PlayerBody.transform.rotation;
                equipment.transform.rotation = newEquipmentRotation;
            }
        }

        ObjectivesSelector.CanWearEquipment = true;
    }
}