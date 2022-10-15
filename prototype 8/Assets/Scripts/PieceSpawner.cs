using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Pieces currentPiece;

    public void Spawn()
    {
        int amtObj = 0;
        switch (type)
        {

            case PieceType.ramp:
                amtObj = Levelmanager.Instance.ramps.Count;
                break;
            case PieceType.longblock:
                amtObj = Levelmanager.Instance.Longblocks .Count;
                break;
            case PieceType.jump:
                amtObj = Levelmanager.Instance.jumps .Count;
                break;
            case PieceType.slide:
                amtObj = Levelmanager.Instance.slides .Count;
                break;
            
        }

        currentPiece = Levelmanager.Instance.GetPieces(type, Random .Range (0,amtObj ));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void Despawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
