# The-Binding-Of-Isaac
Make a Isaac 3W     

오후 12:00 2023-02-13 / New Project Isaac 3D    
오후 12:00 2023-02-13 / Develop test        
오후 18:00 2023-02-14 / Isaac Tile & move      
오후 17:40 2023-02-15 / Isaac Move Animation & Tear        
오후 17:18 2023-02-16 / Fixed Isaac Tear Pool Parent      


=================개발일지 1일차====================

3D Tile map과 2d Player Object를 사용하여 프로젝트를 시작함.    
3D Tile map을 생각한 이유는 아이작의 Istance 방식이 한칸 당 할당하고
Enemy Object 또한 Istance 후 한칸하칸마다 할당 되어 생성 되어 
Tile map을 
=================개발일지 1일차====================

=================개발일지 2일차====================

=================개발일지 2일차====================

=================개발일지 3일차====================

캐릭터 애니메이션 수정	    

아이작의 몸체와 머리의 애니메이션이 부모-자식간의 관계로 이루어짐	    
우 방향 애니메이션은 있고 좌 방향 애니메이션은 없음.	    

1. flip을 써봄. flip 자식 애니메이션은 적용이 안됨	 
flip.X를 사용하여 x축을 중점으로 좌우 반전을 시켜 보았으나      
애니메이션의 자식 애니메이션에는 변동이 없었다.

2.transform.localScale로 적용시켜봄	    

먼저 스케일 크기에 상관없이 방향이 돌아가는지를 확인할려고 Vector3의 키는 1을 줌.       
transform.localScale = new Vector3(-1, 1, 1); // 왼쪽 바라보기	    

transform.localScale = new Vector3(1, 1, 1); // 오른쪽 바라보기	    
 
적용 후 자식 애니메이션도 변경이 되는걸로 확인	    

localSacle을 사용했으니 아이작의 탄환(눈물)도 날아가다가 바로 반전될 수 있으니	    
눈물의 gameObject를 따로 만들어 위치만 붙여줌.	    

=================개발일지 3일차====================


=================개발일지 4일차====================

UnityEditor.Graphs.Edge.WakeUp()    
NullReferenceException: Object reference not set to an instance of an object    
이란 에러가 발생했다.   

어느곳에서도 Null이 나오는곳이 없어서 10분정도 원인 분석 후 검색을 해보니   

애니메이터 사용시 가끔 발생하는 유니티 오류라   

유니티를 다시 켜면 해결이 된다고해서 하니 해결되었다.   

=================개발일지 4일차====================

