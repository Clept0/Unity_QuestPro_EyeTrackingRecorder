import pandas as pd
import numpy as np
import sys
import csv

def main():
    # filenames = ["calibrationVR_20230629001943", "calibrationVR_20230629002034", "screenStabilizedVR_20230629002118", "worldStabilizedVR_20230629002302"]
    # filenames = ["worldStabilizedVR_20230720150042", "worldStabilizedVR_20230720150301", "worldStabilizedVR_20230720150517"]
    filenames = ["worldStabilizedVRsphere_20230726202152", "worldStabilizedVRsphere_20230726203412", "worldStabilizedVRsphere_20230726203621"]
    for filename in filenames:
        print(filename)
        analyze(filename + '.csv', filename + '_error_data.csv')


def analyze(input_csv, output_csv):
    gaze_data = pd.read_csv(input_csv)
    gaze_data = preprocess(gaze_data)
    gaze_data = calc_gaze_point(gaze_data)
    gaze_data = calc_cosine_error(gaze_data)
    gaze_data = calc_euclidean_error(gaze_data)

    gaze_error_data = gaze_data

    gaze_error_data.to_csv(output_csv)


def preprocess(df):
    result = df.loc[(df['Movement'] != "start") & (df['Movement'] != "transition")]
    result.reset_index(inplace=True, drop=True)
    return result

def calc_gaze_point(df):
 
    for i in range(len(df)):
        left_pos = np.array([df.loc[i, 'Gaze_Left Eye Position_x'], df.loc[i, 'Gaze_Left Eye Position_y'], df.loc[i, 'Gaze_Left Eye Position_z']])
        right_pos = np.array([df.loc[i, 'Gaze_Right Eye Position_x'], df.loc[i, 'Gaze_Right Eye Position_y'], df.loc[i, 'Gaze_Right Eye Position_z']])
        
        left_forward = np.array([df.loc[i, 'Gaze_Left Forward Vector_x'], df.loc[i, 'Gaze_Left Forward Vector_y'], df.loc[i, 'Gaze_Left Forward Vector_z']])
        right_forward = np.array([df.loc[i, 'Gaze_Right Forward Vector_x'], df.loc[i, 'Gaze_Right Forward Vector_y'], df.loc[i, 'Gaze_Right Forward Vector_z']])
        
        ball_pos = np.array([df.loc[i, 'Ball Position_x'], df.loc[i, 'Ball Position_y'], df.loc[i, 'Ball Position_z']])
        
        gaze_point = calc_vector_intersection(left_pos, right_pos, left_forward, right_forward)

        df.loc[i, 'Gaze Point_x'] = gaze_point[0][0]
        df.loc[i, 'Gaze Point_y'] = gaze_point[0][1]
        df.loc[i, 'Gaze Point_z'] = gaze_point[0][2]
        
        df.loc[i, 'Debug_Intersection'] = gaze_point[1]
        df.loc[i, 'Debug_Divergence'] = gaze_point[2]
        
    return df

def calc_vector_intersection(p1, p2, r1, r2):
    # err_intersection = 1
    # err_divergence = 0

    # p12 = p1 - p2

    # t1 = 0.0
    # t2 = 0.0

    # r2dotr2 = np.dot(r2, r2)
    # r1dotr1 = np.dot(r1, r1)
    # r1dotr2 = np.dot(r1, r2)

    # denom = pow(r1dotr2, 2) - (r1dotr1 * r2dotr2)

    # if(r1dotr2 < sys.float_info.epsilon or abs(denom) < sys.float_info.epsilon):
    #     err_intersection = 0
    
    # t2 = ((np.dot(p12, r1) * r2dotr2) - (np.dot(p12, r2) * r1dotr2)) / denom
    # t1 = (np.dot(p12, r2) + t2 * r1dotr1) / r1dotr2

    # if (t1 < 0 or t2 < 0):
    #     err_divergence = 1

    # pa = p1 + t1 * r1
    # pb = p2 + t2 * r2

    # pm = (pa + pb) / 2
    # return pm, err_intersection, err_divergence

    intersection = 1
    divergence = 0

    p21 = p1 - p2
    r2dotr2 = np.dot(r2, r2)
    r2dotr1 = np.dot(r2, r1)
    r1dotr1 = np.dot(r1, r1)

    denom = pow(r2dotr1, 2) - (r1dotr1 * r2dotr2)

    if(abs(r2dotr1) < sys.float_info.epsilon or abs(denom) < sys.float_info.epsilon):
        intersection = 0
        return [float('NaN'), float('NaN'), float('NaN')], intersection, divergence
        
    t2 = ((np.dot(p21, r1) * r2dotr2) - (np.dot(p21, r2) * r2dotr1)) / denom
    t1 = (np.dot(p21, r1) + (t2 * r1dotr1)) / r2dotr1
    
    if (t1 < 0 or t2 < 0):
        divergence = 1

    pa = p1 + t1 * r1
    pb = p2 + t2 * r2

    pm = (pa + pb) / 2
    return pm, intersection, divergence


def calc_cosine_error(df):
    # df['Expected_Left Gaze_x'] = df['Gaze_Left Eye Position_x'] - df['Ball Position_x']
    # df['Expected_Left Gaze_y'] = df['Gaze_Left Eye Position_y'] - df['Ball Position_y']
    # df['Expected_Left Gaze_z'] = df['Gaze_Left Eye Position_z'] - df['Ball Position_z']

    # df['Expected_Right Gaze_x'] = df['Gaze_Right Eye Position_x'] - df['Ball Position_x']
    # df['Expected_Right Gaze_y'] = df['Gaze_Right Eye Position_y'] - df['Ball Position_y']
    # df['Expected_Right Gaze_z'] = df['Gaze_Right Eye Position_z'] - df['Ball Position_z']

    # df['Left_Expected Visual Angle_x'] = np.arctan2(df['Expected_Left Gaze_x'], df['Expected_Left Gaze_z'])
    # df['Left_Expected Visual Angle_y'] = np.arctan2(df['Expected_Left Gaze_y'], df['Expected_Left Gaze_z'])

    # df['Right_Expected Visual Angle_x'] = np.arctan2(df['Expected_Right Gaze_x'], df['Expected_Right Gaze_z'])
    # df['Right_Expected Visual Angle_y'] = np.arctan2(df['Expected_Right Gaze_y'], df['Expected_Right Gaze_z'])

    # df['Left_Gaze Visual Angle_x'] = np.arctan2(df['Gaze_Left Forward Vector_x'], df['Gaze_Left Forward Vector_z'])
    # df['Left_Gaze Visual Angle_y'] = np.arctan2(df['Gaze_Left Forward Vector_y'], df['Gaze_Left Forward Vector_z'])
    
    # df['Right_Gaze Visual Angle_x'] = np.arctan2(df['Gaze_Right Forward Vector_x'], df['Gaze_Right Forward Vector_z'])
    # df['Right_Gaze Visual Angle_y'] = np.arctan2(df['Gaze_Right Forward Vector_y'], df['Gaze_Right Forward Vector_z'])

    # df['Left_Cosine Similarity'] = np.degrees(np.arccos((np.dot(np.array([df['Left_Gaze Visual Angle_x'], df['Left_Gaze Visual Angle_y']]), np.array([df['Left_Expected Visual Angle_x'], df['Left_Expected Visual Angle_y']])))/(np.linalg.norm(np.array([df['Left_Gaze Visual Angle_x'], df['Left_Gaze Visual Angle_y']])) * np.linalg.norm(np.array([df['Left_Expected Visual Angle_x'], df['Left_Expected Visual Angle_y']])))))
    # df['Right_Cosine Similarity'] = np.degrees(np.arccos((np.dot(np.array([df['Right_Gaze Visual Angle_x'], df['Right_Gaze Visual Angle_y']]), np.array([df['Right_Expected Visual Angle_x'], df['Right_Expected Visual Angle_y']])))/(np.linalg.norm(np.array([df['Right_Gaze Visual Angle_x'], df['Right_Gaze Visual Angle_y']])) * np.linalg.norm(np.array([df['Right_Expected Visual Angle_x'], df['Right_Expected Visual Angle_y']])))))
    
    # df['Avg_Cosine Similarity'] = df[['Left_Cosine Similarity', 'Right_Cosine Similarity']].mean(axis=1) 
    
    # construct expected gaze vector: center eye position from camera, and ball
    df['Expected Gaze_x'] = df['Camera_Position_x'] - df['Ball Position_x']
    df['Expected Gaze_y'] = df['Camera_Position_y'] - df['Ball Position_y']
    df['Expected Gaze_z'] = df['Camera_Position_z'] - df['Ball Position_z']

    # construct actual gaze vector: center eye position by averaging eye positions, and gaze point
    df['Actual Gaze_x'] = df[['Gaze_Left Eye Position_x', 'Gaze_Right Eye Position_x']].mean(axis=1) - df['Gaze Point_x']
    df['Actual Gaze_y'] = df[['Gaze_Left Eye Position_y', 'Gaze_Right Eye Position_y']].mean(axis=1) - df['Gaze Point_y']
    df['Actual Gaze_z'] = df[['Gaze_Left Eye Position_z', 'Gaze_Right Eye Position_z']].mean(axis=1) - df['Gaze Point_z']

    # # calc visual angles
    # df['Expected Visual Angle_x'] = df.apply(lambda row: np.arctan2(row['Expected Gaze_x'], row['Expected Gaze_z']), axis=1)
    # df['Expected Visual Angle_y'] = df.apply(lambda row: np.arctan2(row['Expected Gaze_y'], row['Expected Gaze_z']), axis=1)

    # df['Actual Visual Angle_x'] = df.apply(lambda row: np.arctan2(row['Actual Gaze_x'], row['Actual Gaze_z']), axis=1)
    # df['Actual Visual Angle_y'] = df.apply(lambda row: np.arctan2(row['Actual Gaze_y'], row['Actual Gaze_z']), axis=1)

    # calc cosine similarity
    for i in range(len(df)):
        # actual_visual_angle = np.array([df.loc[i, 'Actual Visual Angle_x'], df.loc[i, 'Actual Visual Angle_y']])
        # expected_visual_angle = np.array([df.loc[i, 'Expected Visual Angle_x'], df.loc[i, 'Expected Visual Angle_y']])
        # actual_dot_expected = np.dot(actual_visual_angle, expected_visual_angle)
        # actual_visual_angle_norm = np.linalg.norm(actual_visual_angle)
        # expected_visual_angle_norm = np.linalg.norm(expected_visual_angle)
        # cosine_similarity = np.degrees(np.arccos(actual_dot_expected / (actual_visual_angle_norm * expected_visual_angle_norm)))   
        # df.loc[i, 'Cosine Similarity'] = cosine_similarity

        actual_gaze = np.array([df.loc[i, 'Actual Gaze_x'], df.loc[i, 'Actual Gaze_y'], df.loc[i, 'Actual Gaze_z']])
        expected_gaze = np.array([df.loc[i, 'Expected Gaze_x'], df.loc[i, 'Expected Gaze_y'], df.loc[i, 'Expected Gaze_z']])
        actual_dot_expected = np.dot(actual_gaze, expected_gaze)
        actual_gaze_norm = np.linalg.norm(actual_gaze)
        expected_gaze_norm = np.linalg.norm(expected_gaze)
        cosine_similarity = np.degrees(np.arccos(actual_dot_expected / (actual_gaze_norm * expected_gaze_norm)))   
        df.loc[i, 'Cosine Similarity'] = cosine_similarity

    
    # df['Cosine Similarity'] = df.apply(lambda row: np.degrees(np.arccos(np.dot(np.array([row['Actual Visual Angle_x'], row['Actual Visual Angle_y']]), np.array([row['Expected Visual Angle_x'], row['Expected Visual Angle_y']]))/(np.linalg.norm(np.array([row['Actual Visual Angle_x'], row['Actual Visual Angle_y']])) * np.linalg.norm(np.array([row['Expected Visual Angle_x'], df['Expected Visual Angle_y']]))))), axis=1)
    avg_cosine_similarity = df['Cosine Similarity'].mean()
    print("    cosine similarity:", avg_cosine_similarity)
    return df

def calc_euclidean_error(df):
    for i in range(len(df)):
        actual_gaze_visual_angle_x = np.arctan2(df.loc[i, 'Actual Gaze_x'], df.loc[i, 'Actual Gaze_z'])
        actual_gaze_visual_angle_y = np.arctan2(df.loc[i, 'Actual Gaze_y'], df.loc[i, 'Actual Gaze_z'])
        
        expected_gaze_visual_angle_x = np.arctan2(df.loc[i, 'Expected Gaze_x'], df.loc[i, 'Expected Gaze_z'])
        expected_gaze_visual_angle_y = np.arctan2(df.loc[i, 'Expected Gaze_y'], df.loc[i, 'Expected Gaze_z'])
        
        x_dist = actual_gaze_visual_angle_x - expected_gaze_visual_angle_x
        y_dist = actual_gaze_visual_angle_y - expected_gaze_visual_angle_y
        
        x_dist = (x_dist + np.pi) % (np.pi*2) - np.pi
        y_dist = (y_dist + np.pi) % (np.pi*2) - np.pi
        
        euclidean_error = np.degrees(np.sqrt(np.square(x_dist) + np.square(y_dist)))
        df.loc[i, 'Euclidean Error'] = euclidean_error

    # df['Euclidean Error'] = df.apply(lambda row: np.sqrt(np.square(row['Gaze Point_x'] - row['Ball Position_x']) + np.square(row['Gaze Point_y'] - row['Ball Position_y']) + np.square(row['Gaze Point_z'] - row['Ball Position_z'])))
    
    avg_euclidean_error = df['Euclidean Error'].mean()
    print("    euclidean error:", avg_euclidean_error)
    return df

if __name__ == "__main__":
    main()