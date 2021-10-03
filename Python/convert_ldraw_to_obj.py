"""
This is a blender script that converts ldraw format to obj so that the Lego
piece models can be imported into Unity. It also converts them using a few
modifications including various resolutions and bevels. It also simulates
damaged pieces by randomly jittering vertices. This variety ensures more
robust models when used in training image recognition models.

To get started download LDraw from https://www.ldraw.org/
Modify the PATH_TO_LDRAW and OUT_PATH.

Open Blender -> Scripts -> New and paste the script there and let it run.
"""
import os
import bpy
import bmesh
import json
import itertools
import subprocess
from collections import ChainMap
from random import uniform
from mathutils import Vector


PATH_TO_LDRAW = os.path.expanduser('~/Downloads/ldraw/')
OUT_PATH = os.path.expanduser('~/Downloads/ldraw_obj/')
TOP_30_PARTS_IDS = [
    "4073", "3023", "3024", "2780", "98138", "3069b", "3004", "54200", "3710", "3005",
    "3020", "3022", "6558", "15573", "2412b", "3070b", "3021", "3623", "3666", "3003",
    "3010", "11477", "3001", "85984", "4274", "2431", "2420", "3062b", "15068", "85861",
]


def delete_all_objects():
    for obj in bpy.data.objects:
        if obj.name != "Camera" and obj.name != "Lamp" :
            bpy.context.view_layer.objects.active = obj
            bpy.ops.object.mode_set(mode='OBJECT')
            obj.select_set(True)
            bpy.ops.object.delete()


def randomize_mesh(obj, delta=0.004, frac_move=0.002, n_consec_points=20):
    obj.select_set(True) 
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.mode_set(mode='EDIT', toggle=False)
    
    mesh = obj.data
    bm = bmesh.from_edit_mesh(mesh)
    i = n_consec_points
    for v in bm.verts:
        if not v.select:
            continue
        if uniform(0, 1.0) <= frac_move:
            i = n_consec_points
        if i >= 0:
            v.co.xy += Vector([uniform(-delta, delta) for axis in "xy"])
            i -= 1
    bpy.ops.mesh.select_all(action='SELECT')
    bmesh.update_edit_mesh(mesh)
    

def randomize_objects(frac_move):
    for obj in bpy.data.objects:
        if obj.name != "Camera" and obj.name != "Lamp":
            randomize_mesh(obj, frac_move=frac_move)


def save_model(out_path):
    #bpy.ops.export_scene.fbx(filepath=out_path, axis_forward='-Z', axis_up='Y')
    bpy.ops.export_scene.obj(filepath=out_path, axis_forward='-Z', axis_up='Y', use_normals=True, use_uvs=True, use_materials=False)
   

os.makedirs(OUT_PATH, exist_ok=True)
resolution = [{'res': 'Low'}, {'res': 'Standard'}, {'res': 'High'}]
use_logo = [{'logo': True}, {'logo': False}]
bevel_edges = [{'bevel': False}, {'bevel': True}]
randomize = [{'random': False}, {'random': True}]
for part_id in TOP_30_PARTS_IDS:
    idx = 0
    #part_id = '3001'
    for row in itertools.product(randomize, bevel_edges, use_logo, resolution):
        params = dict(ChainMap(*row))
        if params['random'] and params['bevel']:
            continue
        
        # Uncomment to override
        #params = {'res': 'Low', 'logo': False, 'bevel': False, 'random': True}
        
        # Clear and import.
        delete_all_objects()
        dat_path = os.path.join(PATH_TO_LDRAW, 'parts', f'{part_id}.dat')
        bpy.ops.import_scene.importldraw(
            filepath=dat_path,
            useLogoStuds=params['logo'],
            bevelEdges=params['bevel'],
            bevelWidth=1.0,
            resPrims=params['res'],
            smoothParts=True,
            curvedWalls=True,
            addEnvironment=False,
            flatten=True,
            linkParts=False,
            importScale=0.004,
        )
        if params['random']:
            frac_move = 0.004 if params['logo'] else 0.008
            randomize_objects(frac_move=frac_move)
            
        # Save out model.
        out_path = os.path.join(OUT_PATH, f'{part_id}-{idx}.obj')
        save_model(out_path)
        idx += 1
