#!/bin/sh

COMMIT_EDITMSG=$1
TYPE=$2 # one of message, template, merge, squash, commit
REF=$3 # optional, when TYPE is commit

TAGS=""

Files=$(git diff --name-only --cached)
for Line in $Files;
do
    tag=''
    tag=$(python .git/hooks/getPrefixFromPath.py $Line)
    len=`echo $TAGS |awk '{print length}'`
    if [ $len -gt 1 ]; then
        TAGS+=","
    fi
    TAGS+="$tag";
done

printf "[$TAGS] $(cat $COMMIT_EDITMSG)" > $COMMIT_EDITMSG