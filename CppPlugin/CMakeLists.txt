cmake_minimum_required(VERSION 3.6)
project(CppPlugin)

set(SOURCE_FILES Plugin.cpp)
set(TARGET_PATH ${CMAKE_HOME_DIRECTORY}/../Unity/Assets/Plugins)
set(CMAKE_SHARED_LIBRARY_PREFIX "")
set(CMAKE_SHARED_LIBRARY_SUFFIX ".bundle")
set(LIBRARY_OUTPUT_PATH ${TARGET_PATH})

add_library(${PROJECT_NAME} SHARED ${SOURCE_FILES})

